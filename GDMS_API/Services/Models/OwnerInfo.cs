namespace GDMS_API.Models
{
    public class OwnerInfos
    {
        public List<OwnerInfo> ownerInformation { get; set; }


}
    public class OwnerInfo
    {
        public string nameAr { get; set; }
        public string id { get; set; }
        public string nationality { get; set; }
        public string phoneNumber { get; set; }
    }
}
