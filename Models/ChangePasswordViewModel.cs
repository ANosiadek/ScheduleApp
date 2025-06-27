using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    public string UserId { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "Niezgodne hasła!")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
