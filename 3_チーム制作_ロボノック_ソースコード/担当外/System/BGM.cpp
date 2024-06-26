#include "Sound.h"



BGM::BGM(LPCSTR filePath, bool loop)
{
	HRESULT hr;

	//オーディオファイルを開く
	HANDLE hFile = CreateFile(filePath, GENERIC_READ, FILE_SHARE_READ | XAUDIO2_VOICE_NOSRC, NULL, OPEN_EXISTING, 0, NULL);
	SetFilePointer(hFile, 0, NULL, FILE_BEGIN);


	//ファイルの種類を確認
	DWORD dwChunkSize;
	DWORD dwChunkPosition;
	_FindChunk(hFile, fourccRIFF, dwChunkSize, dwChunkPosition);

	DWORD dwFileType;
	_ReadChunkData(hFile, &dwFileType, sizeof(DWORD), dwChunkPosition);
	

	//fmtチャンク を WAVEFORMATEXTENSIBLE構造体 にコピー
	_FindChunk(hFile, fourccFMT, dwChunkSize, dwChunkPosition);
	_ReadChunkData(hFile, &m_wfx, dwChunkSize, dwChunkPosition);


	//dataチャンク をバッファーに読み取り
	_FindChunk(hFile, fourccDATA, dwChunkSize, dwChunkPosition);
	
	BYTE* pDataBuffer = new BYTE[dwChunkSize];
	_ReadChunkData(hFile, pDataBuffer, dwChunkSize, dwChunkPosition);
	

	//XAUDIO2_BUFFER構造体 に値を設定
	m_buffer.AudioBytes = dwChunkSize; //オーディオバッファーのサイズ(バイト単位)
	m_buffer.pAudioData = pDataBuffer; //バッファーにデータを格納
	m_buffer.Flags = XAUDIO2_END_OF_STREAM;
	if (loop) { m_buffer.LoopCount = XAUDIO2_LOOP_INFINITE; }


	//ソース音声を作成
	hr = _GetXAudio2()->CreateSourceVoice(&m_pSourceVoice, (WAVEFORMATEX*)&m_wfx);
	if (FAILED(hr))
	{
		MessageBox(NULL, "SourceVoiceの作成に失敗", "BGM.cpp", MB_OK | MB_ICONERROR);
	}

	//ソース音声に XAUDIO2_BUFFER を送信
	hr = m_pSourceVoice->SubmitSourceBuffer(&m_buffer);
	if (FAILED(hr))
	{
		MessageBox(NULL, "XAUDIO2_BUFFERの送信に失敗", "BGM.cpp", MB_OK | MB_ICONERROR);
	}
}

BGM::~BGM()
{
	m_pSourceVoice->FlushSourceBuffers();
	delete m_buffer.pAudioData;
	m_pSourceVoice->DestroyVoice();
}



void BGM::Start()
{
	m_pSourceVoice->Start();
}

void BGM::Stop()
{
	m_pSourceVoice->Stop();
}

void BGM::SetVolume(float volume)
{
	if (volume < 0) { volume = 0; }
	if (volume > 1) { volume = 1; }

	m_pSourceVoice->SetVolume(volume);
}

void BGM::SetPitch(float pitch)
{
	m_pSourceVoice->SetFrequencyRatio(pitch);
}

