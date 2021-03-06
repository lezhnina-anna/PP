// Lab1_1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <vector>
#include <fstream>
#include "time.h"

void readMatrix(std::vector<std::vector<int>> & matrix, int n)
{
	std::ifstream in("input.txt");
	for (int i = 0; i < n; i++) 
	{
		for (int j = 0; j < n; j++)
		{
			in >> matrix[i][j];
		}
	}
}

void printMatrix(std::vector<std::vector<int>> const & matrix, int n) {
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < n; j++)
		{
			std::cout << matrix[i][j] << " ";
		}
		std::cout << "\n";
	}
}

std::vector<std::vector<int>> getMinorMatrix(std::vector<std::vector<int>> & matrix, int n, int col, int row)
{
	std::vector<std::vector<int>> result;
	result.assign(n-1, std::vector<int>(n-1));
	int ni = 0, nj = 0;
	for (int i = 0; i < n; i++) 
	{
		if (i != row) 
		{
			for (int j = 0; j < n; j++)
			{
				if (j != col)
				{
					result[ni][nj] = matrix[i][j];
					nj++;
				}
			}
			nj = 0;
			ni++;
		}
	
	}
	return result;
}

std::vector<std::vector<int>> getTransponMatrix(std::vector<std::vector<int>> & matrix, int n)
{
	std::vector<std::vector<int>> result;
	result.assign(n, std::vector<int>(n));
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < n; j++)
		{
			result[i][j] = matrix[j][i];
		}

	}
	return result;
}

int calcDet(std::vector<std::vector<int>> & matrix, int n)
{
	int res = 0;
	int mult = 1;
	if (n < 1) 
	{
		return 0;
	}
	if (n == 1) 
	{
		res = matrix[0][0];
	}
	else if (n == 2) {
		res = matrix[0][0] * matrix[1][1] - matrix[1][0] * matrix[0][1];
	}
	else
	{
		for (int i = 0; i < n; i++) {
			std::vector<std::vector<int>> tempMatr;
			tempMatr.assign(n - 1, std::vector<int>(n - 1));
			tempMatr = getMinorMatrix(matrix, n, i, 0);
			res = res + mult * matrix[0][i] * calcDet(tempMatr, n - 1);
			mult = -mult;
		}
	}
	return res;
}

std::vector<std::vector<int>> getInverseMatrix(std::vector<std::vector<int>> & matrix, int n) {
	int det = calcDet(matrix, n);
	std::vector<std::vector<int>> result;
	result.assign(n, std::vector<int>(n));
	std::vector<std::vector<int>> temp;
	temp.assign(n - 1, std::vector<int>(n - 1));
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			temp = getMinorMatrix(matrix, n, i, j);
			result[i][j] = pow(-1.0, i + j + 2) * calcDet(temp, n - 1) / det;
		}
	}
	return result;
}

int main(int argc, char **argv)
{
	if (argc < 2) {
		std::cout << "invalid arguments\n";
	}
	int n = atoi(argv[1]);
	std::vector<std::vector<int>> matrix;
	matrix.assign(n, std::vector<int>(n));
	readMatrix(matrix, n);
	int start = clock();
	getInverseMatrix(matrix, n);
	std::cout << clock() - start << "ms\n";
    return 0;
}

