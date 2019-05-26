#pragma once
#include <iostream>
#include <vector>

#include "BankClient.h"

class CBank
{
public:
	CBank();
	CBankClient* CreateClient();
	void UpdateClientBalance(CBankClient& client, int value);
	int GetTotalBalance();

private:
	std::vector<CBankClient> m_clients;
	int m_totalBalance;

	void SetTotalBalance(int value);
	void SomeLongOperations();
};