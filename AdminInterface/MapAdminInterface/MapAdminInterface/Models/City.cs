using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    [Serializable]
    [MetadataType(typeof(City_MetaData))]
    [DisplayName("Stad")]
    public partial class City
    {
    }

    [Serializable]
    public class City_MetaData
    {
    }
}