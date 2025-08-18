namespace ConcessionariaAPP.Application.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using ConcessionariaAPP.Application.Interfaces;
using ConcessionariaAPP.Domain.Interfaces;
using ConcessionariaAPP.Domain.Entities;
using ConcessionariaAPP.Application.Dto;
using ConcessionariaAPP.Application.Excepetions;
using AutoMapper;

public class SaleAppService : ISaleService
{
    private readonly ISaleRepository _SaleRepository;
    private readonly IMapper _mapper;
    public SaleAppService(ISaleRepository SaleRepository, IMapper mapper)
    {
        _SaleRepository = SaleRepository;
        _mapper = mapper;
    }

    public async Task<SaleDto> CreateAsync(SaleDto dto)
    {
        Validate(dto, isUpdate: false);
        var entity = _mapper.Map<Sales>(dto);

        entity.IsDeleted = false;

        entity.SaleProtocol = GenerateProtocol();


        var created = await _SaleRepository.CreateAsync(entity);
        return _mapper.Map<SaleDto>(created);
    }

    public async Task<SaleDto> UpdateAsync(SaleDto dto)
    {
        Validate(dto, isUpdate: true);

        var entity = _mapper.Map<Sales>(dto);
        var updated = await _SaleRepository.UpdateAsync(entity);
        return _mapper.Map<SaleDto>(updated);
    }

    public async Task<SaleDto> GetByIdAsync(int id)
    {
        var entity = await _SaleRepository.GetByIdAsync(id);
        return _mapper.Map<SaleDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new AppValidationException().Add(nameof(SaleDto.SaleId), "Id inválido para exclusão.");
        }
        return await _SaleRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<SaleDto>> GetAllAsync()
    {
        var list = await _SaleRepository.GetAllAsync();
        return [.. list.Select(e => _mapper.Map<SaleDto>(e))];
    }

    private static void Validate(SaleDto dto, bool isUpdate)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }
        if (isUpdate && dto.SaleId <= 0)
        {
            throw new AppValidationException().Add(nameof(dto.SaleId), "Id inválido para atualização.");
        }

    }
    
    private static string GenerateProtocol()
    {

        var numericString = RemoveNonNumericCharacters(Guid.NewGuid().ToString());
        return numericString;
    }

    private static string RemoveNonNumericCharacters(string value)
    {
        return new string([.. value.Where(char.IsDigit)])[..20];
    }
    

}