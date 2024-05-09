#include "UI_TitleLogo.h"
#include "Renderer.h"
#include "Shader.h"
#include "Sprite.h"

/***************************************************************************
[ŠT—v]
‰Šú‰»ˆ—

[–ß‚è’l]
void
***************************************************************************/
void UI_TitleLogo::Init()
{
	AddComponent<Shader>()->Load("shader/unlitTextureVS.cso", "shader/unlitTexturePS.cso");
	AddComponent<Sprite>()->Init(
		(int)SCREEN_WIDTH_HALF - 400, 100,
		(int)SCREEN_WIDTH_HALF + 400, 300, "asset/texture/Title/TitleLogo.png");
}