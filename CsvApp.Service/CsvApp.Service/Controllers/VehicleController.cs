using CsvApp.Service.Models;
using CsvApp.Service.Services.VehicleService;
using Microsoft.AspNetCore.Mvc;

namespace CsvApp.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Get all vehicles
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> Get(CancellationToken cancellationToken = default)
        {
            var result = await _vehicleService.GetAllVehicles(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific vehicle by ID
        /// </summary>
        /// <param name="id">The ID of the vehicle</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var vehicle = await _vehicleService.GetVehicleById(id, cancellationToken);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        /// <summary>
        /// sends a new Vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Vehicle>> Post([FromBody] Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newVehicle = await _vehicleService.AddVehicle(vehicle, cancellationToken);
            if (newVehicle == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return CreatedAtAction(nameof(Get), new { id = newVehicle }, newVehicle);
        }

        /// <summary>
        /// Update Vehicle by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (vehicle.Id != vehicle.Id)
            {
                return BadRequest();
            }

            var updatedVehicle = await _vehicleService.UpdateVehicle(vehicle, cancellationToken);

            if (!updatedVehicle)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Delete Vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _vehicleService.DeleteVehicle(id, cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Query vehicles based on make and type
        /// </summary>
        /// <param name="make">Make of the vehicle</param>
        /// <param name="type">Type of the vehicle</param>
        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> Query([FromQuery] string make, [FromQuery] string type, CancellationToken cancellationToken = default)
        {
            var result =  await _vehicleService.QueryVehicles(make, type, cancellationToken);

            if (result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
