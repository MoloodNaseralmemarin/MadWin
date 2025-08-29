

namespace MadWin.Core.Entities.Properties
{
    public class PropertyTitle
    {
        public PropertyTitle()
        {
                
        }
        public int PropertyTitleId { get; set; }
        public string Title { get; set; }

        public  List<Property> Properties { get; set; }


    }
}
