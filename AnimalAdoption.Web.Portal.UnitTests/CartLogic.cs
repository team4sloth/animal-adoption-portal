using AnimalAdoption.Common.Logic;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using Xunit;

namespace AnimalAdoption.Service.Cart.UnitTests
{
    public class CartTests
    {
        [Fact]
        public void CartManagement_EmptyCartAddAnimal_AnAnimalIsAdded()
        {
            // Arrange
            var animalId = 1;
            var numberOfAnimalsToAdd = 1;
            var numberOfExpectedAnimalsInCart = 1;

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Act
            var resultingCart = new CartService(memoryCache, new AnimalService()).SetAnimalQuantity("TEST_CART", animalId, quantityAmount);

            // Assert
            Assert.Equal("TEST_CART", resultingCart.Id);
            Assert.Equal(numberOfExpectedAnimalsInCart, resultingCart.CartContents.First(x=>x.Id == animalId).Quantity);
        }

        [Fact]
        public void CartManagement_EmptyCartAddNegativeAnimal_AnAnimalDoesNotGoIntoNegative()
        {
            // Arrange
            var animalId = 1;
            var quantityAmount = -1;

            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Act
            var resultingCart = new CartService(memoryCache, new AnimalService()).SetAnimalQuantity("TEST_CART", animalId, quantityAmount);

            // Assert
            Assert.Equal("TEST_CART", resultingCart.Id);
            Assert.Equal(0, resultingCart.CartContents.First(x => x.Id == animalId).Quantity);
        }

       [Fact]
        public void CartManagement_ExistingAnimalsInCartRemoveAnimal_AnAnimalIsRemoved()
        {
            // Arrange
            var existingAnimalOneId = 1;
            var existingAnimalTwoId = 2;
            var existingAnimalOneQuantityAmount = 1;
            var existingAnimalTwoQuantityAmount = 1;
            var animalIdToRemove = 2;
            var animalQuantityToRemove = -1;
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var animalService = new AnimalService();
            var cartService = new CartService(memoryCache, animalService);
            cartService.SetAnimalQuantity("TEST_CART", existingAnimalOneId, existingAnimalOneQuantityAmount);
            cartService.SetAnimalQuantity("TEST_CART", existingAnimalTwoId, existingAnimalTwoQuantityAmount);

            // Act
            var updatedCart = cartService.SetAnimalQuantity("TEST_CART", animalIdToRemove, animalQuantityToRemove);

            // Assert
            Assert.Equal("TEST_CART", updatedCart.Id);
            Assert.Equal(0, updatedCart.CartContents.First(x => x.Id == animalIdToRemove).Quantity);
            Assert.Equal(0, updatedCart.CartContents.First(x => x.Id == existingAnimalTwoId).Quantity);
            Assert.Equal(1, updatedCart.CartContents.First(x => x.Id == existingAnimalOneId).Quantity);
        }

        [Fact]
        public void SetAnimalQuantity_WithMultipleAnimalWithSameIdInCartAndNegativeUpdateQuantity_RemovesAllAnimalsWithSameIdFromCart()
        {
            // Arrange
            var existingAnimalId = 1;
            var existingAnimalQuantityAmount = 3;
            var animalIdToRemove = 1;
            var numberOfAnimalsToRemove = -1;
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var animalService = new AnimalService();
            var cartService = new CartService(memoryCache, animalService);
            cartService.SetAnimalQuantity("TEST_CART", existingAnimalId, existingAnimalQuantityAmount);

            // Act
            var updatedCart = cartService.SetAnimalQuantity("TEST_CART", animalIdToRemove, numberOfAnimalsToRemove);
            
            // Assert
            Assert.Equal("TEST_CART", updatedCart.Id);
            Assert.Equal(0, updatedCart.CartContents.First(x => x.Id == existingAnimalId).Quantity);
        }
    }
}
