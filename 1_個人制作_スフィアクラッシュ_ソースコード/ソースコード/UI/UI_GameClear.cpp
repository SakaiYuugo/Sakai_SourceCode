#include "UI_GameClear.h"
#include "Renderer.h"
#include "Shader.h"
#include "Sprite.h"

/***************************************************************************
[概要]
初期化処理

[戻り値]
void
***************************************************************************/
void UI_GameClear::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso", "shader/unlitTexturePS.cso");
	AddComponent<Sprite>()->Init(
		0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, "asset/texture/GameClear.png"
	);
}