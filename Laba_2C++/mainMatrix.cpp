#include <iostream>
#include "MatrixData.h"

int main() {
    try {
        std::vector<std::vector<double>> mat1Data = {{1, 2}, {3, 4}};
        std::vector<std::vector<double>> mat2Data = {{15, 6}, {7, 8}};
        

        MyMatrix mat1(mat1Data);
        MyMatrix mat2(mat2Data);

        

        std::string res = mat1.toString();
        std::cout << "Matrix 1:\n" << res << std::endl;

        std::cout<< "Matrix 2:\n"<< mat2.toString() << std::endl;

        MyMatrix sum = mat1.add(mat2);
        MyMatrix product = mat1.multiply(mat2);

        std::cout << "Sum of mat1 and mat2:\n" << sum.toString() << std::endl;
        std::cout << "Product of mat1 and mat2:\n" << product.toString() << std::endl;

        MyMatrix transposed = mat1.getTransposedCopy();
        std::cout << "Transposed Matrix 1:\n" << transposed.toString() << std::endl;

        double* det = mat1.CalculateDeterminant();
        if (det) {
            std::cout << "Determinant of Matrix 1: " << *det << std::endl;
        }

    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }

    return 0;
}