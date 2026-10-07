using System.ComponentModel.DataAnnotations;

namespace Kt12Profiles.ViewModels;

public class ProfileForm
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Заполните информацию о пользователе")]
    [StringLength(1000, ErrorMessage = "Описание не должно быть длиннее 1000 символов")]
    public string Bio { get; set; } = "";

    [StringLength(100, ErrorMessage = "Город не должен быть длиннее 100 символов")]
    public string? City { get; set; }
}

public class UserForm : ProfileForm
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    [StringLength(100, ErrorMessage = "Имя не должно быть длиннее 100 символов")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Введите электронную почту")]
    [EmailAddress(ErrorMessage = "Введите корректную электронную почту")]
    [StringLength(100, ErrorMessage = "Почта не должна быть длиннее 100 символов")]
    public string Email { get; set; } = "";
}

public class HomeModel
{
    public int UsersCount { get; set; }
    public int ProfilesCount { get; set; }
}
