namespace LMS.Models
{
    public class Repository : Audit
    {
        private LmsContext context;

        public Repository()
        {
            this.context = new LmsContext();
        }

        public User? getUser(User user)
        {
            var result = context.Users.SingleOrDefault(record => record.Id == user.Id && record.Password == user.Password);
            return result;
        }

        public List<Book> GetBooks()
        {
            List<Book> listOfBooks = context.Books.ToList();
            List<IssuedBook> issuedBooks = context.IssuedBooks.ToList();
            List<RequestBook> requestBooks = context.RequestBooks.ToList();

            List<Book> result = new List<Book>();
            foreach (Book book in listOfBooks)
            {
                bool isAvailable = true;
                foreach (IssuedBook issue in issuedBooks)
                {
                    if (book.Iban == issue.Iban)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                if (isAvailable)
                {
                    foreach (RequestBook request in requestBooks)
                    {
                        if (request.Iban == book.Iban)
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                }
                if (isAvailable)
                    result.Add(book);
            }
            return result;
        }

        public void AddBook(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
        }
        public int AddBook(string userId, string iban)
        {
            int count1 = context.IssuedBooks.Where(record => record.UserId == userId).Count();
            int count2 = context.RequestBooks.Where(record => record.UserId == userId).Count();
            if (count1 == 1 && count2 == 1)
                return 1;
            if (count1 == 2)
                return 2;
            if (count2 == 2)
                return 3;
            RequestBook data = new RequestBook();
            data.Iban = iban;
            data.UserId = userId;
            context.RequestBooks.Add(data);
            context.SaveChanges();
            return 0;
        }


        public void DeleteBook(string Iban)
        {
            var result = context.Books.Find(Iban);
            if (result != null)
            {
                context.Remove(result);
                context.SaveChanges();
            }
        }

        public bool isBookAlreadyExist(string Iban)
        {
            var result = context.Books.Find(Iban);
            if (result != null)
                return true;
            return false;
        }

        public void editBook(Book book)
        {
            var result = context.Books.Find(book.Iban);
            if (result != null)
            {
                result.Name = book.Name;
                result.Author = book.Author;
                context.SaveChanges();
            }
        }

        private void deleteRequest(RequestBook request)
        {
            var result = context.RequestBooks.SingleOrDefault(record => record.UserId == request.UserId && record.Iban == request.Iban);
            if (result != null)
            {
                context.RequestBooks.Remove(result);
                context.SaveChanges();
            }
        }
        public void approveRequest(RequestBook request)
        {
            IssuedBook book = new IssuedBook();
            book.UserId = request.UserId;
            book.Iban = request.Iban;
            context.IssuedBooks.Add(book);
            context.SaveChanges();
            deleteRequest(request);
        }

        public List<RequestBook> GetRequests()
        {
            return context.RequestBooks.ToList();
        }

        public bool ChangePassword(Password password, string? currUserID)
        {
            var record = context.Users.SingleOrDefault(record => record.Id == currUserID);
            if (record != null && record.Password == password.CurrentPassword)
            {
                record.Password = password.NewPassword;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public string GetUserPassword(string userId)
        {
            var result = context.Users.SingleOrDefault(record => record.Id == userId);
            if (result != null)
            {
                return result.Password;
            }
            return "";
        }

        public List<IssuedBook> GetIssuedBooks()
        {
            return context.IssuedBooks.ToList();
        }

        public List<Book> GetIssuedBooks(string userId)
        {
            List<IssuedBook> issuedBooks = context.IssuedBooks.Where(i => i.UserId == userId).ToList();
            List<Book> allBookList = context.Books.ToList();
            List<Book> result = new List<Book>();

            foreach (IssuedBook issuedBook in issuedBooks)
            {
                foreach (Book book in allBookList)
                {
                    if (book.Iban == issuedBook.Iban)
                    {
                        result.Add(book);
                    }
                }
            }
            return result;
        }

        private bool isUserExist(User user)
        {
            return context.Users.Any(record => record.Id == user.Id);
        }

        public bool AddUser(User user)
        {
            if (!isUserExist(user))
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<User> GetUsers()
        {
            return context.Users.Where(record => record.Admin == false).ToList();
        }

        public bool DeleteUser(string id)
        {
            var result = context.Users.Find(id);
            var isUserIssuedBook = context.IssuedBooks.SingleOrDefault(record => record.UserId == id);
            if (result != null && isUserIssuedBook == null)
            {
                context.Users.Remove(result);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public void returnBook(string userId, string iban)
        {
            var result = context.IssuedBooks.SingleOrDefault(x => x.Iban == iban);
            context.IssuedBooks.Remove(result);
            context.SaveChanges();
        }

        public void I()
        {
            var l = context.Set<RequestBook>();
            context.RemoveRange(l);
            context.SaveChanges();
        }
    }

}
