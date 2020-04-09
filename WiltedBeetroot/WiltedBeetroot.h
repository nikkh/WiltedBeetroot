// MathLibrary.h - Contains declarations of math functions
#pragma once


#define WILTEDBEETROOT_API __declspec(dllexport)
#include <comdef.h>

extern "C" WILTEDBEETROOT_API unsigned calculate_cube(unsigned num);

// Nicks Test string Function
extern "C" WILTEDBEETROOT_API BSTR get_library_name();