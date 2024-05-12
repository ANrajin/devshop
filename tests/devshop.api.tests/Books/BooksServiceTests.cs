using Autofac.Extras.Moq;
using AutoMapper;
using devshop.api.Cores.UnitOfWorks;
using devshop.api.Cores.Utilities;
using devshop.api.Features.Books;
using devshop.api.Features.Books.Repositories;
using devshop.api.Features.Books.Requests;
using devshop.api.Features.Books.Services;
using Moq;
using Shouldly;

namespace devshop.api.tests.Books;

public class BooksServiceTests
{
    private AutoMock? _mock;
    private Mock<IMapper>? _mapperMock;
    private Mock<IUnitOfWorks>? _unitOfWorksMock;
    private Mock<IBookRepository> ?_bookRepositoryMock;
    private Mock<IDateTimeProvider>? _dateTimeProviderMock;
    private IBooksService? _booksServiceMock;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mock = AutoMock.GetLoose();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _mock?.Dispose();
    }

    [SetUp]
    public void ClassSetup()
    {
        _mapperMock = _mock?.Mock<IMapper>();
        _unitOfWorksMock = _mock?.Mock<IUnitOfWorks>();
        _bookRepositoryMock = _mock?.Mock<IBookRepository>();
        _dateTimeProviderMock = _mock?.Mock<IDateTimeProvider>();
        _booksServiceMock = _mock?.Create<BooksService>();
    }

    [TearDown]
    public void ClassTearDown()
    {
        _mapperMock.Reset();
        _unitOfWorksMock.Reset();
        _bookRepositoryMock.Reset();
        _dateTimeProviderMock.Reset();
        _booksServiceMock = null;
    }

    [Test, Category("Unit Test")]
    public async Task GetAllBooks_NoBooksFound_ReturnsEmptyBookResponseCollection()
    {
        //Arrange
        var booksCollection = new List<Book>();
        var booksResponse = new List<BooksResponse>();

        _unitOfWorksMock?.Setup(x => x.BookRepository)
            .Returns(_bookRepositoryMock?.Object!).Verifiable();

        _bookRepositoryMock?.Setup(x => x.GetAllAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(booksCollection).Verifiable();

        _mapperMock?.Setup(x => x.Map<IReadOnlyCollection<BooksResponse>>(booksCollection))
            .Returns(booksResponse)
            .Verifiable();

        //Act
        var books = await _booksServiceMock?.GetAllBooks()!;

        //Assert
        this.ShouldSatisfyAllConditions(
            () => _mapperMock?.VerifyAll(),
            () => _unitOfWorksMock?.VerifyAll(),
            () => _bookRepositoryMock?.VerifyAll(),
            () => books.ShouldBeEmpty(),
            () => books.ShouldNotBeNull());
    }

    [Test, Category("Unit Test")]
    public async Task GetAllBooks_BooksFound_ReturnsBookResponseCollection()
    {
        //Arrange
        var booksCollection = Books();
        var booksResponse = BooksResponse(booksCollection);

        _unitOfWorksMock?.Setup(x => x.BookRepository)
            .Returns(_bookRepositoryMock?.Object!).Verifiable();

        _bookRepositoryMock?.Setup(x => x.GetAllAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(booksCollection).Verifiable();

        _mapperMock?.Setup(x => x.Map<IReadOnlyCollection<BooksResponse>>(booksCollection))
            .Returns(booksResponse)
            .Verifiable();

        //Act
        var books = await _booksServiceMock?.GetAllBooks()!;

        //Assert
        this.ShouldSatisfyAllConditions(
            () => _mapperMock?.VerifyAll(),
            () => _unitOfWorksMock?.VerifyAll(),
            () => _bookRepositoryMock?.VerifyAll(),
            () => books.ShouldNotBeNull(),
            () => books.ShouldNotBeEmpty(),
            () => books.Equals(booksCollection));
    }

    [Test, Category("Unit Test")]
    public async Task GetBook_InvalidIdProvide_ThrowException()
    {
        //Arrange, Act & Assert
        await Should.ThrowAsync<ArgumentException>(
            async () => await _booksServiceMock?.GetBook(Guid.Empty)!);
    }

    [Test, Category("Unit Test")]
    public async Task GetBook_BookNotFound_ThrowException()
    {
        //Arrange
        var bookId = Guid.NewGuid();

        _unitOfWorksMock?.Setup(x => x.BookRepository)
            .Returns(_bookRepositoryMock?.Object!)
            .Verifiable();

        _bookRepositoryMock?.Setup(x => x.GetByIdAsync(bookId))
            .ReturnsAsync(null as Book)
            .Verifiable();

        //Act & Assert
        await Should.ThrowAsync<ArgumentException>(
            async () => await _booksServiceMock?.GetBook(bookId)!);
    }

    [Test, Category("Unit Test")]
    public async Task GetBook_BookFoundById_ReturnBook()
    {
        //Arrange
        var bookList = Books();
        var bookResponseList = BooksResponse(bookList);


        _unitOfWorksMock?.Setup(x => x.BookRepository).Returns(_bookRepositoryMock?.Object!)
            .Verifiable();

        _bookRepositoryMock?.Setup(x => x.GetByIdAsync(bookList.First().Id))
            .ReturnsAsync(bookList.First())
            .Verifiable();

        _mapperMock?.Setup(x => x.Map<BooksResponse>(bookList.First()))
            .Returns(bookResponseList.First())
            .Verifiable();

        //Act
        var book = await _booksServiceMock?.GetBook(bookList.First().Id)!;

        //Assert
        this.ShouldSatisfyAllConditions(
            () => _unitOfWorksMock?.VerifyAll(),
            () => _bookRepositoryMock?.VerifyAll(),
            () => _mapperMock?.VerifyAll(),
            () => book.ShouldNotBeNull(),
            () => book.Equals(bookList.First()));
    }

    [Test, Category("Unit Test")]
    public async Task InsertBooksAsync_BookCreateRequestNotNull_ExecuteInsertBookAsync()
    {
        //Arrange
        var book = Books().First();
        var bookCreateRequest = new BooksCreateRequest(book.Name, book.Price, book.Description, book.PublishedAt.ToDateTime(TimeOnly.MinValue));

        _unitOfWorksMock?.Setup(x => x.BookRepository)
            .Returns(_bookRepositoryMock?.Object!)
            .Verifiable();

        _bookRepositoryMock?.Setup(x => x.InsertAsync(book, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mapperMock?.Setup(x => x.Map<Book>(bookCreateRequest))
            .Returns(book)
            .Verifiable();

        //Act
        await _booksServiceMock?.InsertBooksAsync(bookCreateRequest)!;

        //Assert
        this.ShouldSatisfyAllConditions(
            () => _unitOfWorksMock?.VerifyAll(),
            () => _bookRepositoryMock?.VerifyAll(),
            () => _mapperMock?.VerifyAll());
    }

    private static IReadOnlyCollection<Book> Books()
    {
        var dateTime = new DateTime(2000, 02, 20);

        return
        [
            new() 
            {
                Name = "Book One",
                Description = "This is a sample description",
                Price = 100,
                CreatedAt = DateTime.Now,
                PublishedAt = DateOnly.FromDateTime(dateTime),
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Name = "Book Two",
                Description = "This is a sample description",
                Price = 200,
                CreatedAt = DateTime.Now,
                PublishedAt = DateOnly.FromDateTime(dateTime),
                UpdatedAt = DateTime.Now
            },
            new()
            {
                Name = "Book Three",
                Description = "This is a sample description",
                Price = 300,
                CreatedAt = DateTime.Now,
                PublishedAt = DateOnly.FromDateTime(dateTime),
                UpdatedAt = DateTime.Now
            }
        ];
    }

    private static IReadOnlyCollection<BooksResponse> BooksResponse(IReadOnlyCollection<Book> books)
    {
        var booksResponseList = new List<BooksResponse>();

        foreach (var book in books)
        {
            booksResponseList.Add(new BooksResponse
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                Price = book.Price,
                PublishedAt = book.HumanizedPublishedDate()
            });
        }

        return booksResponseList;
    }
}
