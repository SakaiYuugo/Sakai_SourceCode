#include "Input.h"
#include "WinUtil.h"

//--- グローバル変数
BYTE g_keyTable[256];
BYTE g_oldTable[256];

HRESULT InitInput()
{
	// 一番最初の入力
	GetKeyboardState(g_keyTable);
	return S_OK;
}
void UninitInput()
{
}
void UpdateInput()
{
	// 古い入力を更新
	memcpy_s(g_oldTable, sizeof(g_oldTable), g_keyTable, sizeof(g_keyTable));
	// 現在の入力を取得
	GetKeyboardState(g_keyTable);
}

bool IsKeyPress(BYTE key)
{
	return g_keyTable[key] & 0x80;
}
bool IsKeyAnyTrigger()
{
	bool Anser;
	for (int i = 0; i < 256; i++)
	{
		Anser = (g_keyTable[i] ^ g_oldTable[i]) & g_keyTable[i] & 0x80;
		if (Anser)
		{
			return true;
		}
	}

	return false;
}
bool IsKeyTrigger(BYTE key)
{
	return (g_keyTable[key] ^ g_oldTable[key]) & g_keyTable[key] & 0x80;
}
bool IsKeyRelease(BYTE key)
{
	return (g_keyTable[key] ^ g_oldTable[key]) & g_oldTable[key] & 0x80;
}
bool IsKeyRepeat(BYTE key)
{
	return false;
}

int GetMousePosX()
{
	return (int)mousePosX;
}

int GetMousePosY()
{
	return (int)mousePosY;
}
