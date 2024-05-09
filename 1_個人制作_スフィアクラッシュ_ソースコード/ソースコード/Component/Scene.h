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
		// ���C���[��؂�ւ���
		for (auto& objectList : m_GameObject)
		{
			// ���C���[�̃Q�[���I�u�W�F�N�g
			for (GameObject* object : objectList)
			{		
				//----- �q�b�g�X�g�b�v���邩����
				if ((GameObject::GetHitStopBit() & object->GetObjectBit()) != 0x00)
				{
					//----- �q�b�g�X�g�b�v�p�A�b�v�f�[�g
					object->HitStopUpdateBase();
					continue;
				}

				//----- �ʏ�̃A�b�v�f�[�g
				object->UpdateBase();
			}

			//----- �폜�t���O�������Ă��镨���폜
			objectList.remove_if([](GameObject* object) {return object->Destroy(); });
		}

		//----- 2D�Q�[���I�u�W�F�N�g
		for (auto& objectList : m_GameObject_2D)
		{
			// ���C���[�̃Q�[���I�u�W�F�N�g
			for (GameObject* object : objectList)
			{
				//----- �q�b�g�X�g�b�v���邩����
				if ((GameObject::GetHitStopBit() & object->GetObjectBit()) != 0x00)
				{
					//----- �q�b�g�X�g�b�v�p�A�b�v�f�[�g
					object->HitStopUpdateBase();
					continue;
				}

				//----- �ʏ�̃A�b�v�f�[�g
				object->UpdateBase();
			}

			//----- �폜�t���O�������Ă��镨���폜
			objectList.remove_if([](GameObject* object) {return object->Destroy(); });
		}

		Update();
	}

	void DrawBase()
	{
		DirectX::XMFLOAT4X4 matrix;
		DirectX::XMStoreFloat4x4(&matrix, DirectX::XMMatrixIdentity());

		//----- �J�������Ȃ��ꍇ�ʏ�`��
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
			//----- �J����������ꍇ�AZ�o�b�t�@���l�������`��
			DirectX::XMFLOAT3 CameraNormal;  // �J�����̖@��
			DirectX::XMFLOAT3 CameraPosition = m_pCamera->GetPosition();

			CameraNormal = m_pCamera->GetTarget() - CameraPosition;
			CameraNormal = VectorNormalize(CameraNormal);

			//----- ���ʂ̕�����
			DirectX::XMFLOAT4 plane;
			plane.x = CameraNormal.x;
			plane.y = CameraNormal.y;
			plane.z = CameraNormal.z;
			plane.w = -(plane.x * CameraPosition.x + plane.y * CameraPosition.y + plane.z * CameraPosition.z);

			//----- �J�������狗�����������ɕ��ёւ�
			std::vector <CameraLengthObject> CameraLengthObj;

			//----- �J��������̋����𒲂ׂ�
			for (auto& objectList : m_GameObject_2D)
			{
				for (GameObject* object : objectList)
				{
					DirectX::XMFLOAT3 pos = object->GetPosition();
					float length = (plane.x * pos.x + plane.y * pos.y + plane.z * pos.z + plane.w);

					//----- �I�u�W�F�N�g�̏�����
					CameraLengthObject temp;
					temp.point = object;
					temp.length = length;

					CameraLengthObj.push_back(temp);
				}
			}

			//----- �J�����̉������Ƀ\�[�g
			std::sort(CameraLengthObj.begin(), CameraLengthObj.end(),
				[](const CameraLengthObject& copyA, const CameraLengthObject& copyB)
				{
					return copyA.length > copyB.length;
				}
			);

			int layer = 0;

			for (auto& objectList : m_GameObject)
			{
				//----- �ʏ�`��
				for (GameObject* object : objectList)
				{
					object->DrawBase(matrix);
				}

				//----- ���C���[��2�Ԃ̏ꍇ�AZ�o�b�t�@���l�������`��
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
	template <typename T> //�e���v���[�g�֐�
	T* AddGameObject(int Layer = 2)
	{
		T* gameObject = new T();
		m_GameObject[Layer].push_back(gameObject);
		gameObject->Init();

		return gameObject;
	}

	template <typename T> //�e���v���[�g�֐�
	T* AddGameObject2D(int Layer = 2)
	{
		T* gameObject = new T();
		m_GameObject_2D[Layer].push_back(gameObject);
		gameObject->Init();

		return gameObject;
	}

	template <typename T> //�e���v���[�g�֐�
	T* AddGameCamera(int Layer = 2)
	{
		T* gameObject = new T();
		m_GameObject[Layer].push_back(gameObject);
		gameObject->Init();
		
		//----- �J�����̏�����
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
				if (typeid(*object) == typeid(T))//�^�𒲂ׂ�(RTTI���I�^���)
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
		std::vector<T*> objects;//STL�̔z��
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
