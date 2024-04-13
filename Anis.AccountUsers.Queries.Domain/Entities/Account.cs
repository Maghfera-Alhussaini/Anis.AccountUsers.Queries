namespace Anis.AccountUsers.Queries.Domain.Entities
{
    public class Account
    {
        private Account(Guid id, int sequence)
        {
            Id = id;
            Sequence = sequence;
        }

        public Guid Id { get; private set; }
        public int Sequence { get; private set; }
        public List<User> Users { get; private set; } = new List<User>();

        public static Account Create(Guid id, int sequence)
        {
            var account = new Account(id, sequence);

            return account;
        }

        public void IncreamentSequence()
            => Sequence++;
    }
}
