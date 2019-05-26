#pragma once
#include <iostream>
#include <vector>
#include <mutex>

#include "BankClient.h"

class CBank
{
public:
	CBank(int csNumber);
	CBankClient* CreateClient();
	void UpdateClientBalance(CBankClient& client, int value);
	int GetTotalBalance();

private:
	std::vector<CBankClient> m_clients;
	int m_totalBalance;
	int m_csNumber;

	CRITICAL_SECTION criticalSection;
	std::mutex mutex;

	void SetTotalBalance(int value);
	void SomeLongOperations();
	void CBank::StartCS();
	void CBank::LeaveCS();
};