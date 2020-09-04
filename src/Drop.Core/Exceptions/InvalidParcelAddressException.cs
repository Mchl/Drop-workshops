namespace Drop.Core.Exceptions
{
    public class InvalidParcelAddressException : DomainException
    {
        public override string Code { get; } = "invalid_parcel_address";
        public string Address { get; }

        public InvalidParcelAddressException(string address) : base($"Invalid address: {address}")
        {
            Address = address;
        }
    }
}