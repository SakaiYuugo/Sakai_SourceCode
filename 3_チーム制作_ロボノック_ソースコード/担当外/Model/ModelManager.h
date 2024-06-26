//#pragma once
#ifndef MODEL_MANAGER_H
#define MODEL_MANAGER_H
#include "Model.h"
#include "DirectX.h"
#include "Geometory.h"
#include "Game3D.h"
#include <map>
#include <string>

void ModelManager_Init();
void ModelManager_Uninit();
void AddModel(const std::string CharName, const char* FileName);			//モデルを追加する
void ModelDraw(const std::string CharName);
void ModelDraw(const std::string CharName,float PosX,float PosY,float PosZ,float ScaleX,float ScaleY,float ScaleZ,float AngleX,float AngleY,float AngleZ);
void AllDelete();			//全てのモデル情報を消す

#endif //MODEL_MANAGER_H
