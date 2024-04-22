#pragma once

#include <random>

class Random
{
public:
	/***************************************************************************
	[�T�v]
	�^����������(�����Z���k�E�c�C�X�^�[)
	
	[�߂�l]
	void
	***************************************************************************/
	static void Init();

	/***************************************************************************
	[�T�v]
	�^����������(�����Z���k�E�c�C�X�^�[)
	
	[�߂�l]
	void
	***************************************************************************/
	static int GetInt();

	/***************************************************************************
	[�T�v]
	�^����������(�����Z���k�E�c�C�X�^�[)
	
	[�߂�l]
	void
	***************************************************************************/
	static int GetInt(int min, int max);

	/***************************************************************************
	[�T�v]
	�^����������(�����Z���k�E�c�C�X�^�[)

	[�߂�l]
	void
	***************************************************************************/
	static float GetFloat();

	/***************************************************************************
	[�T�v]
	�^����������(�����Z���k�E�c�C�X�^�[)

	[�߂�l]
	void
	***************************************************************************/
	static float GetFloat(float min, float max);

private:
	static std::mt19937 Generator;
};