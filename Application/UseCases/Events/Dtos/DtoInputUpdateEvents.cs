﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Domain;

namespace Application.UseCases.Events.Dtos;

public class DtoInputUpdateEvents
{
    public string IdEventsEmployee { get; set; }
    public int IdAccount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsValid { get; set; }
    public string? Comments { get; set; }
    public string Types { get; set; }
    
    public EventTypes? EventTypes { get; set; }
}