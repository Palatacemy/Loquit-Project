using Loquit.Data.Repositories;
using Loquit.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Loquit.Data.Entities;
using Microsoft.Extensions.Hosting;

namespace LoquitMVC.Tests
{
    [TestFixture]
    public class PostRepositoryTests
    {
        private ApplicationDbContext _context;
        private PostRepository _postRepository;

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
                new Post { Id = 1, Title = "TestPost1", BodyText = "TestText", Category = "None" },
                new Post { Id = 2, Title = "TestPost2", BodyText = "clean energy future", Category = "Pets"},
                new Post { Id = 3, Title = "TestPost3", BodyText = "happy SAD", Category = "Food" }

            });
            _context.SaveChanges();

            _postRepository = new PostRepository(_context);
        }

        [Test]
        public async Task Algorithm_ShouldReturnInCorrectOrder()
        {
            Post updatedPost = new Post();
            for (int i = 1; i <= _postRepository.GetAllAsync().Result.Count(); i++)
            {
                updatedPost = await _postRepository.GetByIdAsync(i);
                updatedPost.Evaluations = _postRepository.Evaluate(updatedPost);
                await _postRepository.UpdateAsync(updatedPost);
                await _context.SaveChangesAsync();
            }
            int[] recentlyOpenedPostsIds = new int[50];
            for (int i = 0; i < 50; i++)
            {
                recentlyOpenedPostsIds[i] = 0;
            }
            List<Post> result = await _postRepository.GetPostsByAlgorithmAsync(true, [ 0, 1, 0, 0, 0, 0, 0, 0, 0 ], [ 0, 1, 1, 0, 0 ], recentlyOpenedPostsIds);
            Assert.IsNotNull(result);
            Assert.That(result[0].Id == 3 && result[1].Id == 2);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

    }
}