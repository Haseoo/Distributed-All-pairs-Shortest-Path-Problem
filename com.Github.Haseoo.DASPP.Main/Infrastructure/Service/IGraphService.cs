﻿using com.Github.Haseoo.DASPP.Main.Dtos;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Service
{
    public interface IGraphService
    {
        MainTaskResponseDto CalculateBestVertex(MainTaskRequestDto request);
    }
}