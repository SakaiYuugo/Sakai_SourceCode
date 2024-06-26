#pragma once

#define _CRT_SECURE_NO_WARNINGS
#define NOMINMAX
#include <stdio.h>
#include <windows.h>
#include <tchar.h>
#include <assert.h>
#include <functional>



#pragma warning(push)
#pragma warning(disable:4005)

#include <d3d11.h>
#include <DirectXMath.h>

#pragma warning(pop)



#pragma comment (lib, "winmm.lib")
#pragma comment (lib, "d3d11.lib")


#define SCREEN_WIDTH	  (1280)
#define SCREEN_WIDTH_HALF (SCREEN_WIDTH * 0.5f)
#define SCREEN_HEIGHT	  (720)
#define SCREEN_HEIGHT_HALF (SCREEN_HEIGHT * 0.5f)

HWND GetWindow();
void Invoke(std::function<void()> Function, int Time);
