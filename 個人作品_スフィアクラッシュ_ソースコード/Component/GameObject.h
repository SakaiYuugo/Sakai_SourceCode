#pragma once

#include "Renderer.h"

#include <list>
#include <numbers>
#include "Component.h"
#include "Quaternion.h"

class GameObject
{
public:
	enum class TAG_BIT : unsigned int
	{
		NONE      = 0x00, // 0000
		Player    = 0x01, // 0001
		Enemy     = 0x02, // 0010
		UI        = 0x04, // 0100
		ALLOBHECT = 0xffffffff,  // 32�o�C�g
	};

private:
	static int m_HitStopTagBit; // �q�b�g�X�g�b�v����I�u�W�F�N�g���r�b�g�ŊǗ�
	static int m_HitStopTime;   // �q�b�g�X�g�b�v����t���[����

public:
	static void SetHitStopTime(int time) { m_HitStopTime = time; }
	static int GetHitStopTime() { return m_HitStopTime; }
	static int GetHitStopBit() { return m_HitStopTagBit; }

	static void SetHitStopBit(TAG_BIT bit)
	{
		m_HitStopTagBit = m_HitStopTagBit | static_cast<int>(bit); // �q�b�g�X�g�b�v����I�u�W�F�N�g��ǉ�
	}

	static void ResetHitStopBit(TAG_BIT bit)
	{
		m_HitStopTagBit = m_HitStopTagBit & ~static_cast<int>(bit); // �r�b�g�����Z�b�g
	}

protected:
	DirectX::XMFLOAT3 m_Position = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);   // ���ݍ��W
	DirectX::XMFLOAT3 m_PrevPos  = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);   // �O�t���[�����W
	DirectX::XMFLOAT3 m_Rotation = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);   // ��]�l
	DirectX::XMFLOAT3 m_Scale    = DirectX::XMFLOAT3(1.0f, 1.0f, 1.0f);   // �傫��
	DirectX::XMFLOAT3 m_Velocity = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);   // ���x
	DirectX::XMFLOAT3 m_Forward  = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);   // �O���x�N�g��
	DirectX::XMFLOAT3 m_Torque   = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);   // ��]��

	DirectX::XMFLOAT3 m_ChildPosition = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	DirectX::XMFLOAT3 m_ChildRotation = DirectX::XMFLOAT3(0.0f, 0.0f, 0.0f);
	DirectX::XMFLOAT3 m_ChildScale = DirectX::XMFLOAT3(1.0f, 1.0f, 1.0f);

	std::list<Component*> m_Component;
	std::list<GameObject*> m_ChildGameObject;

	Quaternion m_Quaternion;

	float m_Time           = 0.0f;       
	float m_Accel          = 0.0f; 
	float m_ShootPower     = 0.0f; 
	float m_RotForce       = 0.0f;
	float m_EffectDrawTime = 0.0f; // �G�t�F�N�g�̕`�掞��(�t���[����)

	bool m_Moving          = false;  // �ړ���������
	bool m_Charge          = false;	 // �U��������������
	bool m_Attack          = false;	 // �U����������
	bool m_Destroy         = false;  // �I�u�W�F�N�g�̍폜����
	bool m_SoundPlay       = false;  // SE���Đ���������
	bool m_EffectPlay      = false;  // �G�t�F�N�g��`�撆������
	bool m_PlayerCollision = false;  // �v���C���[�ɏՓ˂��Ă��邩����

	//----- �q�b�g�X�g�b�v�֌W
	int m_TagBit = 0x00;    // �r�b�g�ŃI�u�W�F�N�g���Ǘ�
	int m_FrameCount = 0;   // ���t���[���q�b�g�X�g�b�v���邩�J�E���g�p
	bool m_HitStop = false; // �q�b�g�X�g�b�v���Ă��邩����

	//----- �萔
	const DirectX::XMFLOAT3 m_OriginPoint = { 0.0f, 0.0f, 0.0f }; // �t�B�[���h�̒��S���W
	const float m_AdditionTime   =  0.01f;      // �I�u�W�F�N�g���̃t���[����
	const float m_Friction       =  0.964f;     // ���C
	const float m_Gravity        =  0.015f;     // �d��
	const float m_AccelForce     =  0.001f;     // ������
	const float m_Threshold      =  0.05f;      // ���x��臒l�ȉ��̏ꍇ�ړ��\�ɂ���
	const float m_DeadPosY       = -10.0f;      // �t�B�[���h�̎��S������W
	const float m_MaxRotForce    =  45.0f;      // �ő��]��
	const float m_MaxSpeed       =  0.18f;      // �ő呬�x
	const float m_MaxAccel       =  0.03f;      // �ő������
	const float m_MaxShootPower  =  1.0f;       // �U�����̍ő������
	const float m_Resistance     =  0.01f;      // Y���W�̒�R�l
	const float m_GroundHeight   =  1.0f;       // �n�ʂ̍��W

public:
	GameObject() {}
	virtual ~GameObject() {}


	DirectX::XMFLOAT3 GetPosition() { return m_Position; }
	DirectX::XMFLOAT3 GetRotation() { return m_Rotation; }
	DirectX::XMFLOAT3 GetScale()    { return m_Scale; }
	DirectX::XMFLOAT3 GetVelocity() { return m_Velocity; }
	DirectX::XMFLOAT3 GetPrevPos()  { return m_PrevPos; }
	float GetShootPower()		    {return m_ShootPower;}

	void SetPosition(DirectX::XMFLOAT3 Position) { m_Position = Position; }
	void SetRotation(DirectX::XMFLOAT3 Rotation) { m_Rotation = Rotation; }
	void SetScale(DirectX::XMFLOAT3 Scale)       { m_Scale    = Scale; }
	void SetVelocity(DirectX::XMFLOAT3 velocity) { m_Velocity = velocity; }
	void SetTorque(DirectX::XMFLOAT3 torque)     { m_Torque   = torque; }

	//----- �q�b�g�X�g�b�v�֌W
	int GetObjectBit() { return m_TagBit; }
	void SetObjectBit(TAG_BIT bit) { m_TagBit |= static_cast<int>(bit); }
	void ResetObjectBit(TAG_BIT bit) { m_TagBit &= ~static_cast<int>(bit); }

	//----- �G�t�F�N�g�֌W
	void SetEffectDrawFrame(float frame) { m_EffectDrawTime = frame; }
	float GetEffectDrawFrame() { return m_EffectDrawTime; }

	//----- �N�H�[�^�j�I�����g�p���邩
	bool m_UseQuaternion = false;


	DirectX::XMFLOAT3 GetForward()   //�O�����x�N�g���擾
	{
		DirectX::XMFLOAT4X4 rot;
		if (m_UseQuaternion)
		{
			DirectX::XMStoreFloat4x4(&rot,
				DirectX::XMMatrixRotationQuaternion(m_Quaternion.v));
		}
		else
		{
			DirectX::XMStoreFloat4x4(&rot,
				DirectX::XMMatrixRotationRollPitchYaw(
					m_Rotation.x, m_Rotation.y, m_Rotation.z));
		}

		DirectX::XMFLOAT3 forward;
		forward.x = rot._31;
		forward.y = rot._32;
		forward.z = rot._33;

		return forward;
	}

	void SetDestroy() { m_Destroy = true; }

	bool Destroy()
	{
		if (m_Destroy)
		{
			UninitBase();
			delete this;
			return true;
		}
		return false;
	}



	virtual void Init() {}
	virtual void Uninit() {}
	virtual void Update() {}
	virtual void HitStopUpdate() {}
	virtual void Draw() {}
	virtual void PreDraw() {}

	template <typename T>
	T* AddComponent()
	{
		T* component = new T(this);
		m_Component.push_back(component);
		((Component*)component)->Init();

		return component;
	}


	template <typename T>
	T* GetComponent()
	{
		for (Component* component : m_Component)
		{
			if (typeid(*component) == typeid(T))
			{
				return (T*)component;
			}
		}
		return nullptr;
	}


	template <typename T>
	T* AddChild()
	{
		T* child = new T();
		m_ChildGameObject.push_back(child);
		child->InitBase();

		return child;
	}



	void InitBase()
	{
		Init();
	}


	void UninitBase()
	{
		Uninit();

		for (Component* component : m_Component)
		{
			component->Uninit();
			delete component;
		}
		m_Component.clear();
	}

	void UpdateBase()
	{
		
		for (Component* component : m_Component)
		{
			component->Update();
		}

		Update();
		
	}

	void HitStopUpdateBase()
	{
		HitStopUpdate();
	}

	void DrawBase(DirectX::XMFLOAT4X4 ParentMatrix)
	{
		PreDraw();

		// �}�g���N�X�ݒ�
		DirectX::XMFLOAT4X4 world;
		DirectX::XMMATRIX scale, rot, trans;
		scale = DirectX::XMMatrixScaling(m_Scale.x, m_Scale.y, m_Scale.z);

		if (m_UseQuaternion)
		{
			// �N�H�[�^�j�I���̉�]�s��
			rot = DirectX::XMMatrixRotationQuaternion(m_Quaternion.v);
		}
		else
		{
			rot = DirectX::XMMatrixRotationRollPitchYaw(m_Rotation.x, m_Rotation.y, m_Rotation.z);
		}

		trans = DirectX::XMMatrixTranslation(m_Position.x, m_Position.y, m_Position.z);
		DirectX::XMStoreFloat4x4(&world, scale * rot * trans * DirectX::XMLoadFloat4x4(&ParentMatrix));

		for (GameObject* child : m_ChildGameObject)
		{
			child->DrawBase(world);
		}

		Renderer::SetWorldMatrix(&world);

		for (Component* component : m_Component)
		{
			component->Draw();
		}

		Draw();
	}
};