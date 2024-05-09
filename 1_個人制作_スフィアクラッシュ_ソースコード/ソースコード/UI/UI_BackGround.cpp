#include "UI_BackGround.h"
#include "Renderer.h"
#include "Shader.h"
#include "Sprite.h"

/***************************************************************************
[�T�v]
����������

[�߂�l]
void
***************************************************************************/
void UI_BackGround::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso", "shader/unlitTexturePS.cso");
	AddComponent<Sprite>()->Init(
		(int)SCREEN_WIDTH_HALF - (int)SCREEN_WIDTH_HALF, 0,
		(int)SCREEN_WIDTH_HALF + (int)SCREEN_WIDTH_HALF, SCREEN_HEIGHT, "asset/texture/Title/background.png");
}