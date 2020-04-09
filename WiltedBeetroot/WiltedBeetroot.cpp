// MathLibrary.cpp : Defines the exported functions for the DLL.
#include "pch.h" // use stdafx.h in Visual Studio 2017 and earlier
#include <utility>
#include <limits.h>
#include "WiltedBeetroot.h"
#include <string>

unsigned WILTEDBEETROOT_API calculate_cube(unsigned num)
{
    return num * num * num;
}

BSTR WILTEDBEETROOT_API get_library_name()
{
    return SysAllocString(L"Library Name is WiltedBeetroot");
}


