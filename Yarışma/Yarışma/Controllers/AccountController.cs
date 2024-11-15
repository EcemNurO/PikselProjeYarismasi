using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Yarışma.Models.Controllers
{
    public class AccountController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Login()
        {
            ViewBag.Message = "";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kullanıcıyı veritabanında bul
            var user = db.usedContestantJudges
                .FirstOrDefault(u => u.Email == model.Email && u.Deleted == false && u.Status == true);

            // Kullanıcı veya şifre yanlışsa
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Geçersiz e-posta veya şifre.");
                return View(model);
            }

            // Kullanıcı doğrulandı, oturumu başlat
            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Email),
    new Claim(ClaimTypes.Role, user.Role.ToString()) // Kullanıcı rolü ekleniyor
};

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(4)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            

            //Kullanıcı rolüne göre yönlendirme
            if (user.Role == RoleTypes.Admin)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (user.Role == RoleTypes.Contestant)
            {
                return RedirectToAction("Profile", "Contest");
            }
          
            
                return RedirectToAction("JudgeProfile", "Judge");
            

            // Eğer rol eşleşmezse varsayılan bir sayfaya yönlendirme
           // return RedirectToAction("JudgeProfile", "Judge");
           
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult RegisterContestant()
        {
            ViewBag.Categories = new SelectList(db.ContestantCategories?.ToList() ?? new List<ContestantCategory>(), "Id", "Name");
            ViewBag.ProjectCategories = new SelectList(db.ProjectCategories?.ToList() ?? new List<ProjectCategory>(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult RegisterContestant(ContestantRegisterViewModel model)
        {
            if (!ModelState.IsValid) 
            { 
    
              
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                ViewBag.Categories = new SelectList(db.ContestantCategories.ToList(), "Id", "Name");
                ViewBag.ProjectCategories = new SelectList(db.ProjectCategories.ToList(), "Id", "Name");
                return View(model);
            }
            var existingUser = db.usedContestantJudges.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
                return View(model);
            }

            // Yeni kullanıcı kaydı oluşturma
            var newUser = new UsedContestantJudge
            {
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = RoleTypes.Contestant,
                Status = true,
                Deleted = false
            };
            db.usedContestantJudges.Add(newUser);
            db.SaveChanges();

            // Profil resmi kaydetme işlemi
            string uniqueFileName = null;
            if (model.image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");
                Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.image.CopyTo(fileStream);
                }
            }

            // Profil kaydı oluşturma
            var profile = new ContestantProfil
            {
                FullName = model.FullName,
                Age = model.Age,
                Phone = model.Phone,
                Email = model.Email,
                Univercity = model.Univercity,
                Address = model.Address,
                image = uniqueFileName != null ? "/images/profiles/" + uniqueFileName : null,
                usedContestantJudgeId = newUser.Id,
                Status=true,
                Deleted=false
            };
            db.ContestantProfils.Add(profile);
            db.SaveChanges();

            // Yarışmacı kaydı oluşturma
            var contestant = new Contestant
            {
                ContestantProfilId = profile.Id,
                ContestantCategoryId = model.ContestantCategoryId,
               Status=true,
               Deleted=true
            };
            db.Contestants.Add(contestant);
            db.SaveChanges();


          

            // Project kaydını oluşturma
            var project = new Project
            {
                Name = model.ProjectName,
                ContestantId = contestant.Id,
                ProjectCategoryId = model.ProjectCategoryId,
                Status=true,
                Deleted = false

            };
            db.Projects.Add(project);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult RegisterJudge()
        {
            ViewBag.JudgeCategories = new SelectList(db.JudgeCategories?.ToList() ?? new List<JudgeCategory>(), "Id", "Name");
            ViewBag.ProjectCategories = new SelectList(db.ProjectCategories?.ToList() ?? new List<ProjectCategory>(), "Id", "Name");

            return View();
        }

            [HttpPost]
        public IActionResult RegisterJudge(JudgeRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
               
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                ViewBag.JudgeCategories = new SelectList(db.JudgeCategories.ToList(), "Id", "Name");
                //ViewBag.ProjectCategories = new SelectList(db.ProjectCategories.ToList(), "Id", "Name");
                return View(model);
            }


            var existingUser = db.usedContestantJudges.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
                return View(model);
            }

            // Yeni kullanıcı kaydı oluşturma
            var newUser = new UsedContestantJudge
            {
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = RoleTypes.Judge,
                Status = true,
                Deleted = false
            };
            db.usedContestantJudges.Add(newUser);
            db.SaveChanges();


            // Profil fotoğrafını kaydet
            string uniqueFileName = null;
            if (model.image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");
                Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.image.CopyTo(fileStream);
                }
            }


            // Hakem profil kaydı oluşturma
            var profile = new JudgeProfil
            {
                FullName = model.FullName,
               
                Phone = model.Phone,
                Email = model.Email,
                Univercity = model.Univercity,
                Address = model.Address,
                image = uniqueFileName != null ? "/images/profiles/" + uniqueFileName : null,
                UsedContestantJudgeId = newUser.Id,
                Status = true,
                Deleted = false
            };
            db.JudgeProfils.Add(profile);
            db.SaveChanges();

            // Hakem kaydı oluşturma
            var judge = new Judge
            {
                JudgeProfilId = profile.Id,
                JudgeCategoryId = (int)model.JudgeCategoryId,
                 ProjectCategoryId = (int)model.ProjectCategoryId,
                Status = true,
                Deleted = false,
                IsApproved = false

                
            };
            db.Judges.Add(judge);
            db.SaveChanges();

            



            return RedirectToAction("Login", "Account");
        }





    }
}
