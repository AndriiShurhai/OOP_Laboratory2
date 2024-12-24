#include <iostream>
#include <vector>
#include <stdexcept>
#include<typeinfo>
#include "MatrixData.h"

class SimpleTestRunner {
private:
    int totalTests = 0;
    int passedTests = 0;

    template<typename T>
    bool assertEqual(T expected, T actual, const std::string& testName) {
        totalTests++;
        if (expected == actual) {
            passedTests++;
            std::cout<< testName << " Passed" << std::endl;
            return true;
        } else {
            std::cout<<typeid(expected).name()<<std::endl;
            std::cout<<typeid(actual).name()<<std::endl;

            std::cout << testName << " Failed" 
                      << "\n   Expected: " << expected 
                      << "\n   Actual: " << actual << std::endl;
            return false;
        }
    }

public:
    void runMatrixTests() {
        testMatrixConstruction();
        
        testMatrixAddition();
        
        testMatrixMultiplication();
        
        testMatrixTranspose();

        testMatrixDeterminant();
        
        printTestSummary();
    }

private:
    void testMatrixConstruction() {
        std::vector<std::vector<double>> data = {{1, 2}, {3, 4}};
        MyMatrix mat(data);

        assertEqual(2, mat.getHeight(), "Matrix Height");
        assertEqual(2, mat.getWidth(), "Matrix Width");
        assertEqual(1.0, mat.getElement(0, 0), "Matrix Element [0,0]");
        assertEqual(4.0, mat.getElement(1, 1), "Matrix Element [1,1]");
    }

    void testMatrixAddition() {
        std::vector<std::vector<double>> data1 = {{1, 2}, {3, 4}};
        std::vector<std::vector<double>> data2 = {{5, 6}, {7, 8}};
        
        MyMatrix mat1(data1);
        MyMatrix mat2(data2);
        
        MyMatrix sum = mat1.add(mat2);
        
        assertEqual(6.0, sum.getElement(0, 0), "Matrix Addition [0,0]");
        assertEqual(12.0, sum.getElement(1, 1), "Matrix Addition [1,1]");
    }

    void testMatrixMultiplication() {
        std::vector<std::vector<double>> data1 = {{1, 2}, {3, 4}};
        std::vector<std::vector<double>> data2 = {{5, 6}, {7, 8}};
        
        MyMatrix mat1(data1);
        MyMatrix mat2(data2);
        
        MyMatrix product = mat1.multiply(mat2);
        
        assertEqual(19.0, product.getElement(0, 0), "Matrix Multiplication [0,0]");
        assertEqual(50.0, product.getElement(1, 1), "Matrix Multiplication [1,1]");
    }

    void testMatrixTranspose() {
        std::vector<std::vector<double>> data = {{1, 2}, {3, 4}};
        
        MyMatrix mat(data);
        MyMatrix transposed = mat.getTransposedCopy();
        
        assertEqual(1.0, transposed.getElement(0, 0), "Transpose [0,0]");
        assertEqual(3.0, transposed.getElement(0, 1), "Transpose [0,1]");
        assertEqual(2.0, transposed.getElement(1, 0), "Transpose [1,0]");
        assertEqual(4.0, transposed.getElement(1, 1), "Transpose [1,1]");
    }

    void testMatrixDeterminant() {
        std::vector<std::vector<double>> data = {{1, 2, 3, 4}, {4, 3, 2, 1}, {4, 5, 6, 7}, {7, 7, 7, 7}};
        std::vector<std::vector<double>> data2 = {{1, 2, 3, 4}, {3, 4, 5, 5}, {2, 8, 7, 6}, {1, 9, 8, 7}};

        MyMatrix mat = MyMatrix(data);
        MyMatrix mat2 = MyMatrix(data2);
        double* result = mat.CalculateDeterminant();
        double* result2 = mat2.CalculateDeterminant();

        bool isFirstMatrixZero = std::abs(*result) < 1e-10;
        bool isSecondMatrixEqualToNegative13 = std::abs(*result2 - (-13.0)) < 1e-10;

        assertEqual(true, isFirstMatrixZero, "First Matrix Determinant is Zero");
        assertEqual(true, isSecondMatrixEqualToNegative13, "Second Matrix Determinant is -13");
    }

    void printTestSummary() {
        std::cout << "\nTest Summary:" << std::endl;
        std::cout << "Total Tests: " << totalTests << std::endl;
        std::cout << "Passed Tests: " << passedTests << std::endl;
        std::cout << "Failed Tests: " << (totalTests - passedTests) << std::endl;
    }
};

int main() {
    SimpleTestRunner testRunner;
    testRunner.runMatrixTests();
    return 0;
}