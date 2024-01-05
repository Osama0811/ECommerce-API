
using CircuitsUc.Application.Common.SharedResources;
using System.ComponentModel.DataAnnotations;

namespace CircuitsUc.Application.Helpers
{
   

    public static class CommenEnum
    {
       
        public enum RoleType
        {
            Admin = 1,
            Customer = 2,
        }
        public enum EntityType
        {
            Admin = 1,
            Customer = 2,
            ProductCategory=3,
            ProductImage=4,
            ProductFile=5,
            SystemParameter=6,
            PageContent=7
        }
        public enum PageType
        {
            [Display(Description = "Services", ResourceType = typeof(GeneralMessages))]
            Services = 1,
            [Display(Description = "Blogs", ResourceType = typeof(GeneralMessages))]
            Blogs = 2,
            [Display(Description = "Partners", ResourceType = typeof(GeneralMessages))]

            Partners = 3  ,
            [Display(Description = "About", ResourceType = typeof(GeneralMessages))]

            About = 4

        }
        public enum SystemParameterKey
        {
            InviteApp,
            ContactInfo,
            AboutApp,
            WebSiteUrl,
            ContactEmailConfiguration
        };
    }
}
