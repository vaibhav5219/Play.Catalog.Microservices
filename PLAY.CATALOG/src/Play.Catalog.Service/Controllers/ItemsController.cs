using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service
{
    [ApiController]
    [Route("Items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository; // creating Repository object
        private static int requestCounter = 0;
        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            requestCounter++;
            Console.WriteLine($"Request {requestCounter}:  Starting...");

            if(requestCounter <= 2)
            {
                Console.WriteLine($"Request {requestCounter}:  Dealying...");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            if(requestCounter <= 4)
            {
                Console.WriteLine($"Request {requestCounter}:  500(Internal Server Error).");
                return StatusCode(500);
            }

            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());

            Console.WriteLine($"Request {requestCounter}:  200(Ok).");
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto createItemDto)
        {
            // var itemDto = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, Convert.ToDecimal(createItemDto.Price), DateTimeOffset.UtcNow);
            // Items.Add(itemDto);
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = Convert.ToDecimal(createItemDto.Price),
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto updateItemDto)
        {
            // var existingItem = Items.Where(item => item.Id == id).SingleOrDefault();

            // if (existingItem == null)
            // {
            //     return NotFound();
            // }

            // var updatedItem = existingItem with
            // {
            //     Name = updateItemDto.Name,
            //     Price = Convert.ToDecimal(updateItemDto.Price),
            //     Description = updateItemDto.Description
            // };

            // var index = Items.FindIndex(existingItem => existingItem.Id == id);
            // Items[index] = updatedItem;

            var existingItem = await itemsRepository.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = Convert.ToDecimal(updateItemDto.Price);
            existingItem.CreatedDate = DateTimeOffset.UtcNow;

            await itemsRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // var index = Items.FindIndex(item => item.Id == id);

            // if (index < 0)
            // {
            //     return NotFound();
            // }
            // Items.RemoveAt(index);

            var item = await itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await itemsRepository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}