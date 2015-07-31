using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

public class MyViewModel
{
    [Required]
    public HttpPostedFileBase File { get; set; }
}