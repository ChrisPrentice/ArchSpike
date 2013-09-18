namespace ILB.Contacts
{
    public class Contact
    {
        public Contact(CreateContactCommand command)
        {
            Populate(command);
        }

        public int Id { get; set; }
        public string FirstName { get; private set;  }
        public string Surname { get; private set; }
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public County County { get; set; }
        public Country Country { get; set; }

        public void Update(UpdateContactCommand command)
        {
            Populate(command);
        }

        private void Populate(CreateContactCommand command)
        {
            FirstName = command.FirstName;
            Surname = command.Surname;
            Address1 = command.Address1;
            Address2 = command.Address2;
        }

        public override string ToString()
        {
            return FirstName + " " + Surname;
        }
    }
}