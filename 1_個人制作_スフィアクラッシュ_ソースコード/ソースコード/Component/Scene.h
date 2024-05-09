#pragma once

#include <array>
#include <list>
#include <vector>
#include <typeinfo>
#include <algorithm>
#include "GameObject.h"
#include "ModelRenderer.h"
#include "Camera.h"
#include "CameraLengthObject.h"
#include "XMFLOAT_Calculation.h"

class Scene
{
protected:
	std::array < std::list<GameObject*>, 5>	m_GameObject;
	std::array < std::list<GameObject*>, 5>	m_GameObject_2D;
	float m_Count = 0.02f;

	Camera* m_pCamera = nullptr;

public:
	Scene() {}
	virtual ~Scene() {}

	virtual void Init() {}
	virtual void Uninit() {}
	virtual void Update() {}
	virtual void Draw() {}

	void InitBase()
	{
		Init();
	}

	void UninitBase()
	{
		for (auto& objectList : m_GameObject)
		{
			for (GameObject* object : objectList)
			{
				object->UninitBase();
				delete object;
			}
			objectList.clear();
		}
	
		Uninit();

	}

	void UpdateBase()
	{
		// レイヤーを切り替える
		for (auto& objectList : m_GameObject)
		{
			// レイヤーのゲームオブジェクト
			for (GameObject* object : objectList)
			{		
				//----- ヒットストップするか判定
				if ((GameObject::GetHitStopBit() & object->GetObjectBit()) != 0x00)
				{
					//----- ヒットストップ用アップデート
					object->HitStopUpdateBase();
					continue;
				}

				//----- 通常のアップデート
				object->UpdateBase();
			}

			//----- 削除フラグが立っている物を削除
			objectList.remove_if([](GameObject* object) {return object->Destroy(); });
		}

		//----- 2Dゲームオブジェクト
		for (auto& objectList : m_GameObject_2D)
		{
			// レイヤーのゲームオブジェクト
			for (GameObject* object : objectList)
			{
				//----- ヒットストップするか判定
				if ((GameObject::GetHitStopBit() & object->GetObjectBit()) != 0x00)
				{
					//----- ヒットストップ用アップデート
					object->HitStopUpdateBase();
					continue;
				}

				//----- 通常のアップデート
				object->UpdateBase();
			}

			//----- 削除フラグが立っている物を削除
			objectList.remove_if([](GameObject* object) {return object->Destroy(); });
		}

		Update();
	}

	void DrawBase()
	{
		DirectX::XMFLOAT4X4 matrix;
		DirectX::XMStoreFloat4x4(&matrix, DirectX::XMMatrixIdentity());

		//----- カメラがない場合通常描画
		if (m_pCamera == nullptr)
		{
			int layer = 0;

			for (auto& objectList : m_GameObject)
			{
				for (GameObject* object : objectList)
				{
					object->DrawBase(matrix);
				}

				if (layer == 2)
				{
					for (auto& objectList : m_GameObject_2D)
					{
						for (GameObject* object : objectList)
						{
							object->DrawBase(matrix);
						}
					}
				}

				layer++;
			}
		}
		else
		{
			//----- カメラがある場合、Zバッファを考慮した描画
			DirectX::XMFLOAT3 CameraNormal;  // カメラの法線
			DirectX::XMFLOAT3 CameraPosition = m_pCamera->GetPosition();

			CameraNormal = m_pCamera->GetTarget() - CameraPosition;
			CameraNormal = VectorNormalize(CameraNormal);

			//----- 平面の方程式
			DirectX::XMFLOAT4 plane;
			plane.x = CameraNormal.x;
			plane.y = CameraNormal.y;
			plane.z = CameraNormal.z;
			plane.w = -(plane.x * CameraPosition.x + plane.y * CameraPosition.y + plane.z * CameraPosition.z);

			//----- カメラから距離が遠い順に並び替え
			std::vector <CameraLengthObject> CameraLengthObj;

			//----- カメラからの距離を調べる
			for (auto& objectList : m_GameObject_2D)
			{
				for (GameObject* object : objectList)
				{
					DirectX::XMFLOAT3 pos = object->GetPosition();
					float length = (plane.x * pos.x + plane.y * pos.y + plane.z * pos.z + plane.w);

					//----- オブジェクトの情報を代入
					CameraLengthObject temp;
					temp.point = object;
					temp.length = length;

					CameraLengthObj.push_back(temp);
				}
			}

			//----- カメラの遠い順にソート
			std::sort(CameraLengthObj.begin(), CameraLengthObj.end(),
				[](const CameraLengthObject& copyA, const CameraLengthObject& copyB)
				{
					return copyA.length > copyB.length;
				}
			);

			int layer = 0;

			for (auto& objectList : m_GameObject)
			{
				//----- 通常描画
				for (GameObject* object : objectList)
				{
					object->DrawBase(matrix);
				}

				//----- レイヤーが2番の場合、Zバッファを考慮した描画
				if (layer == 2)
				{
					for (auto& object : CameraLengthObj)
					{
						object.point->DrawBase(matrix);
					}
				}

				layer++;
			}

		}
		
		Draw();
	}



	// Layer 
	template <typename T> //テンプレート関数
	T* AddGameObject(int Layer = 2)
	{
		T* gameObject = new T();
		m_GameObject[Layer].push_back(gameObject);
		gameObject->Init();

		return gameObject;
	}

	template <typename T> //テンプレート関数
	T* AddGameObject2D(int Layer = 2)
	{
		T* gameObject = new T();
		m_GameObject_2D[Layer].push_back(gameObject);
		gameObject->Init();

		return gameObject;
	}

	template <typename T> //テンプレート関数
	T* AddGameCamera(int Layer = 2)
	{
		T* gameObject = new T();
		m_GameObject[Layer].push_back(gameObject);
		gameObject->Init();
		
		//----- カメラの情報を代入
		m_pCamera = gameObject;

		return gameObject;
	}



	template <typename T>
	T* GetGameObject()
	{
		for (auto& objectList : m_GameObject)
		{
			for (GameObject* object : objectList)
			{
				if (typeid(*object) == typeid(T))//型を調べる(RTTI動的型情報)
				{
					return (T*)object;
				}
			}
		}
		return nullptr;
	}

	template <typename T>
	std::vector<T*> GetGameObjects()
	{
		std::vector<T*> objects;//STLの配列
		for (auto& objectList : m_GameObject)
		{
			for (GameObject* object : objectList)
			{
				if (typeid(*object) == typeid(T))
				{
					objects.push_back((T*)object);
				}
			}
		}
		return objects;
	}
};
