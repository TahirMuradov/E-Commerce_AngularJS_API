namespace Shop.Domain.Exceptions
{
    public class AuthStatusException
    {
        public static Dictionary<string, string> EmailInvalid
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "az", "Email səhvdir!" },
                    { "ru", "Неверный адрес электронной почты!" },
                    { "en", "Incorrect email address!" }
                };
            }
        }
        public static Dictionary<string, string> UserPasswordOrEmailWrong
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "az", "Şifrə vəya Email səhvdir!" },
                    { "ru", "Неверный пароль или адрес электронной почты!" },
                    { "en", "Incorrect password or email address!" }
                };
            }
        }
        public static Dictionary<string, string> UserNotFound
        {
            get
            {
                return new Dictionary<string, string>()
                {
 { "az", "İstifadəçi tapılmadı!" },
  { "ru", "Пользователь не найден!" },
      { "en", "User not found!" },
                };
            }
        }
        public static Dictionary<string, string> RoleNotFound
        {
            get
            {
                return new Dictionary<string, string>()
                {
 { "az", "Rol tapılmadı!" },
  { "ru", "Рол не найден!" },
      { "en", "Rol not found!" },
                };
            }
        }
        public static Dictionary<string, string> EmailAlreadyExists
        {
            get
            {
                return new Dictionary<string, string>()
                {
 { "az", "E-poçt artıq mövcuddur!" },
  { "ru","Email уже используется!" },
  { "en", "Email is already in use!" }
                };
            }
        }
        public static Dictionary<string, string> UserNameAlreadyExists
        {
            get
            {
                return new Dictionary<string, string>()
                {
 { "az", "İstifadəçi adı artıq mövcuddur!" },
  { "ru", "Имя пользователя уже существует!"},
  {"en", "Username already exists!" }
                };
            }
        }
        public static Dictionary<string, string> RegistrationSuccess
        {
            get
            {
                return new Dictionary<string, string>()
                {
{ "az", "Uğurla qeydiyyatdan keçdiniz!" },
      { "ru", "Регистрация прошла успешно!" },
        { "en", "Registration successful!" },
                };
            }
        }
        public static Dictionary<string, string> ConfirmationLinkNotSend
        {
            get
            {
                return new Dictionary<string, string>()
                {
 { "az", "Təsdiq Linki göndərilə bilmədi!Yenidən qeyydiyatdan keçməyə çalışın." },
  { "ru", "Не удалось отправить ссылку для подтверждения. Попробуйте зарегистрироваться еще раз." },
      { "en", "Verification Link could not be sent! Try registering again." },
                };
            }
        }

        public static Dictionary<string, string> InvalidToken
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Etibarsız token!" },
            { "ru", "Недействительный токен!" },
            { "en", "Invalid token!" }
        };
            }
        }
        public static Dictionary<string, string> InvalidTokenForEmailConfirmation
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Tokenin vaxtı bitib.Yenidən qeyydiyatdan keçin!" },
            { "ru", "Время токена закончилось. Пройдите через вновь." },
            { "en", "Token's time is over. Go through the newly." }
        };
            }
        }
        public static Dictionary<string, string> PasswordTooShort
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Şifrə çox qısadır! Minimum {0} simvol olmalıdır." },
            { "ru", "Пароль слишком короткий! Минимум {0} символов." },
            { "en", "Password is too short! Minimum length is {0} characters." }
        };
            }
        }
        public static Dictionary<string, string> PasswordMismatch
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Şifrələr uyğun gəlmir!" },
            { "ru", "Пароли не совпадают!" },
            { "en", "Passwords do not match!" }
        };
            }
        }
        public static Dictionary<string, string> RoleAlreadyExists
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Rol artıq mövcuddur!" },
            { "ru", "Роль уже существует!" },
            { "en", "Role already exists!" }
        };
            }
        }
        public static Dictionary<string, string> RoleCreationFailed
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Rol yaradılarkən xəta baş verdi!" },
            { "ru", "Ошибка при создании роли!" },
            { "en", "An error occurred while creating the role!" }
        };
            }
        }
        public static Dictionary<string, string> AccountLocked
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Hesab kilidlənib. Zəhmət olmasa sonra yenidən cəhd edin!" },
            { "ru", "Учетная запись заблокирована. Пожалуйста, попробуйте позже!" },
            { "en", "Account is locked. Please try again later!" }
        };
            }
        }
        public static Dictionary<string, string> AccessDenied
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Giriş icazəsi yoxdur!" },
            { "ru", "Доступ запрещен!" },
            { "en", "Access denied!" }
        };
            }
        }

        public static Dictionary<string, string> ConcurrencyFailure
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Eyni anda bir neçə dəyişiklik cəhdi aşkarlandı." },
            { "ru", "Обнаружен конфликт одновременных изменений." },
            { "en", "A concurrency failure has occurred." }
        };
            }
        }
        public static Dictionary<string, string> DefaultError
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Naməlum xəta baş verdi." },
            { "ru", "Произошла неизвестная ошибка." },
            { "en", "An unknown failure has occurred." }
        };
            }
        }

        public static Dictionary<string, string> PasswordRequiresUniqueChars
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Şifrə ən az {0} fərqli simvol daxil etməlidir." },
            { "ru", "Пароль должен содержать не менее {0} уникальных символов." },
            { "en", "Passwords must use at least {0} different characters." }
        };
            }
        }

        public static Dictionary<string, string> UserAlreadyHasPassword
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "İstifadəçi artıq şifrə təyin edib." },
            { "ru", "У пользователя уже установлен пароль." },
            { "en", "User already has a password set." }
        };
            }
        }
        public static Dictionary<string, string> RecoveryCodeRedemptionFailed
        {
            get
            {
                return new Dictionary<string, string>()
                {
                     { "az", "Bərpa kodu istifadə edilə bilmədi." },
    { "ru", "Код восстановления не может быть использован." },
    { "en", "Recovery code redemption failed." }
                };
            }
        }
        public static Dictionary<string, string> PasswordRequiresDigit
        {
            get
            {
                return new Dictionary<string, string>()
                {
                     { "az", "Şifrə ən az bir rəqəm içərməlidir." },
    { "ru", "Пароль должен содержать хотя бы одну цифру." },
    { "en", "Passwords must have at least one digit." }
                };
            }
        }
        public static Dictionary<string, string> PasswordRequiresLower
        {
            get
            {
                return new Dictionary<string, string>()
            {
                 { "az", "Şifrə ən az bir kiçik hərf içərməlidir." },
{ "ru", "Пароль должен содержать хотя бы одну строчную букву." },
{ "en", "Passwords must have at least one lowercase letter." }
            };
            }
        }
        public static Dictionary<string, string> PasswordRequiresUpper
        {
            get
            {
                return new Dictionary<string, string>()
            {
                 { "az", "Şifrə ən az bir böyük hərf içərməlidir." },
{ "ru", "Пароль должен содержать хотя бы одну заглавную букву." },
{ "en", "Passwords must have at least one uppercase letter." }
            };
            }
        }
        public static Dictionary<string, string> PasswordRequiresNonAlphanumeric
        {
            get
            {
                return new Dictionary<string, string>()
            {
                 { "az", "Şifrə ən az bir xüsusi simvol içərməlidir." },
{ "ru", "Пароль должен содержать хотя бы один неалфавитный символ" },
{ "en", "Passwords must have at least one non-alphanumeric character." }
            };
            }
        }
        public static Dictionary<string, string> UserAlreadyInRole
        {
            get
            {
                return new Dictionary<string, string>()
            {
                 { "az", "{0} rolu artıq istifadəçiyə təyin olunub." },
{ "ru", "Пользователь уже имеет роль {0}." },
{ "en", "User already has the {0} role." }
            };
            }
        }
        public static Dictionary<string, string> UserLockoutNotEnabled
        {
            get
            {
                return new Dictionary<string, string>()
            {
                 { "az", "İstifadəçi kilidləmə aktiv deyil." },
{ "ru", "Блокировка пользователя не включена." },
{ "en", "User lockout is not enabled." }
            };
            }
        }
        public static Dictionary<string, string> UserNotInRole
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "az", "İstifadəçi {0} rolunda deyil." },
                    { "ru", "Пользователь не имеет роли {0}." },
                    { "en", "User is not in the {0} role." }
                };
            }
        }
        public static Dictionary<string, string> LoginAlreadyAssociated
        {
            get
            {
                return new Dictionary<string, string>()
            {
                 { "az", "Bu xarici giriş artıq başqa bir istifadəçiyə bağlıdır." },
{ "ru", "Пользователь с этим внешним логином уже существует." },
{ "en", "A user with this external login already exists." }
            };
            }
        }
        public static Dictionary<string, string> ConfirmTokenAlreadyUsed
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "Bu təsdiq linki artıq istifadə olunub və ya müddəti bitib." },
            { "ru", "Эта ссылка для подтверждения уже использована или истек срок её действия." },
            { "en", "This confirmation link has already been used or has expired." }
        };
            }
        }
        public static Dictionary<string, string> InvalidUserName
        {
            get
            {
                return new Dictionary<string, string>()
        {
            { "az", "{0} istifadəçi adı etibarsızdır. Yalnız hərflər və rəqəmlər istifadə edilə bilər." },
            { "ru", "Недопустимое имя пользователя {0}. Можно использовать только буквы и цифры." },
            { "en", "Username {0} is invalid, can only contain letters or digits." }
        };
            }
        }
    }
}
