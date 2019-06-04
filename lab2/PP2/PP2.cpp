#include "stdafx.h"
#include "Bank.h"
#include "BankClient.h"
#include "iostream"
#include "string"

void PrintHelpMessage()
{
	std::cout << "Usage: PP2.exe <clients count> <number synchronous primitive>";
	std::cout << "Numbers synchronous primitive: 0 for Critical Section, 1 for Mutex";
	std::cout << "Default: clients count = 2, number synchronous primitive = 0";
}

int main(int argc, char *argv[])
{
	int count = 2, cs = 0;
	if(argc == 3) {
		count = atoi(argv[1]);
		cs = atoi(argv[2]);
		if (count < 1)
		{
			std::cout << "Incorrect clients count";
			return 1;
		}
		if (cs < 0 || cs > 1)
		{
			std::cout << "Incorrect number synchronous primitive";
			return 1;
		}
	} 
	if (argc == 2) {
		if (argv[1] == "/")
		{
			PrintHelpMessage();
		}
		else 
		{
			count = atoi(argv[1]);
			if (count < 1)
			{
				std::cout << "Incorrect clients count";
				return 1;
			}
		}
	}
	CBank* bank = new CBank(cs);
	std::vector<CBankClient> clients;
	for (int i = 0; i < count; i++) {
		CBankClient client = *(bank->CreateClient());
		clients.push_back(client);
	}

	std::string command;
	while (std::cin >> command)
	{
		if (command == "exit" || command == "quit") {
			std::cout << "Bank total balance: " << bank->GetTotalBalance() << std::endl;
		}
	}
	
    return 0;
}