using ShoeShop.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class CustomerViewModel
{
    [Display(Name = "FullName")]
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }

    [Display(Name = "UserName")]
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Display(Name = "Phone")]
    [Required(ErrorMessage = "Phone is required")]
    public string PhoneNumber { get; set; }

    [Display(Name = "BirthDay")]
    [Required(ErrorMessage = "BirthDay is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDay { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Display(Name = "Gender")]
    [Required(ErrorMessage = "Gender is required")]
    public int Gender { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "Status is required")]
    public bool Status { get; set; }
}
