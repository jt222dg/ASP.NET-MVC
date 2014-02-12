using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    [Serializable]
    [MetadataType(typeof(Icon_MetaData))]
    [DisplayName("Ikon")]
    public partial class Icon
    {

    }

    [Serializable]
    public class Icon_MetaData
    {
    }

}