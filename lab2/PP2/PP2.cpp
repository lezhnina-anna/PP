#include "stdafx.h"
#include "Bank.h"
#include "BankClient.h"
#include "iostream"
#include "string"

int main(int argc, char *argv[])
{
	int count = 2;
	if(argc == 2) {
		count = atoi(argv[1]);
	}
	CBank* bank = new CBank();
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