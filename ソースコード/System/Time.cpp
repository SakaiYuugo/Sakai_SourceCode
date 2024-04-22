#include "Time.h"
#include "UI_Timer.h"

//----- �ÓI�ϐ�
DWORD Time::m_dwExecLastTime;
DWORD Time::m_dwCurrentTime;
float Time::m_Frame;
bool  Time::m_HitStopFlg;
int   Time::m_GameCount;

/***************************************************************************
[�T�v]
����������

[�߂�l]
void
***************************************************************************/
void Time::Init()
{
	timeBeginPeriod(1);

	//----- �v���O�����J�n���̎��Ԃ��擾
	m_dwExecLastTime = timeGetTime();

	//----- �ŏ��̃A�b�v�f�[�g�p
	m_dwCurrentTime = 0;

	m_HitStopFlg = false;
}

/***************************************************************************
[�T�v]
�������̉������

[�߂�l]
void
***************************************************************************/
void Time::Uninit()
{
	timeEndPeriod(1);
}

/***************************************************************************
[�T�v]
�X�V����

[�߂�l]
bool 1�b�o�߂��Ă��邩����
***************************************************************************/
bool Time::Update()
{
	//----- ���ݎ��Ԏ擾
	m_dwCurrentTime = timeGetTime();

	//----- UI�Ɍ��ݎ��Ԃ��Z�b�g(�b�P��)
	UI_Timer::SetCurrentTime((m_dwCurrentTime - m_dwExecLastTime) / 1000);

	//----- �o�ߎ��Ԃ��v�Z(�b�P��)
	if ((m_dwCurrentTime - m_dwExecLastTime) / 1000.0f  >= UI_Timer::GetTimeLimit()) 
	{
		m_dwExecLastTime = m_dwCurrentTime;
		return true;
	}

	return false;
}

