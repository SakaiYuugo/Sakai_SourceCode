#include "UI_TitleLogo.h"
#include "Renderer.h"
#include "Shader.h"
#include "Sprite.h"

/***************************************************************************
[概要]
初期化処理

[戻り値]
void
***************************************************************************/
void UI_TitleLogo::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso", "shader/unlitTexturePS.cso");
	AddComponent<Sprite>()->Init(
		(int)SCREEN_WIDTH_HALF - 400, 100,
		(int)SCREEN_WIDTH_HALF + 400, 300, "asset/texture/Title/TitleLogo.png");
}