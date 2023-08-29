using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Aristino.Models;
using AutoMapper;
using Aristino.ViewModel;

namespace Aristino.Controllers
{
    public class AristinoPoliciesController : Controller
    {
        private readonly AristinoDbContext _context;
        private readonly IMapper _mapper;
        public AristinoPoliciesController(AristinoDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: AristinoPolicies
        public async Task<IActionResult> Index(int PolicyID)
        {
            var getPolicy=_context.AristinoPolicies.Where(x=>x.PolicyId== PolicyID).ToListAsync();
            var getPolicyVM = _mapper.Map<Task<List<AristinoPolicyVM>>>(getPolicy);
            return View(await getPolicyVM);
        }
    }
}
