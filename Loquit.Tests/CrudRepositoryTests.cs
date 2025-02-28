using Loquit.Data.Repositories;
using Loquit.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Loquit.Data.Entities;

namespace LoquitMVC.Tests
{
    [TestFixture]
    public class CrudRepositoryTests
    {
        private ApplicationDbContext _context;
        private CrudRepository<Post> _postRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Posts.AddRange(new List<Post>
            {
                new Post { Id = 1, Title = "TestPost1", BodyText = "TestText", PictureUrl = "https://google.com/" },
                new Post { Id = 2, Title = "TestPost2", BodyText = "TestText", PictureUrl = "https://google.com/" }
            });
            _context.SaveChanges();

            _postRepository = new CrudRepository<Post>(_context);
        }

        [Test]
        public async Task CreateAsync_ShouldAddEntity()
        {
            var newPost = new Post { Id = 3, Title = "TestPost3", BodyText = "TestText", PictureUrl = "https://google.com/" };
            await _postRepository.AddAsync(newPost);

            var result = await _context.Posts.FindAsync(3);
            Assert.IsNotNull(result);
            Assert.That(result.Title, Is.EqualTo("TestPost3"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            var posts = await _postRepository.GetAllAsync();
            Assert.That(posts.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectEntity()
        {
            var post = await _postRepository.GetByIdAsync(1);
            Assert.IsNotNull(post);
            Assert.That(post.Title, Is.EqualTo("TestPost1"));
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveEntity_WhenEntityExists()
        {
            await _postRepository.DeleteByIdAsync(1);

            var post = await _context.Posts.FindAsync(1);
            Assert.IsNull(post);
        }

        [Test]
        public void DeleteByIdAsync_ShouldThrowException_WhenEntityDoesNotExist()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _postRepository.DeleteByIdAsync(99));
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyEntity()
        {
            var existingPost = await _postRepository.GetByIdAsync(1);
            existingPost.Title = "UpdatedTitle";
            await _postRepository.UpdateAsync(existingPost);

            var updatedPost = await _context.Posts.FindAsync(1);
            Assert.That(updatedPost.Title, Is.EqualTo("UpdatedTitle"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

    }
}