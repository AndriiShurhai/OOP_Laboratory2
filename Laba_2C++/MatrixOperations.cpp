#include "MatrixData.h"
#include <stdexcept>
#include<cmath>
#include<iostream>

MyMatrix MyMatrix::add(const MyMatrix& other) const{
    if (getHeight() != other.getHeight() || getWidth() != other.getWidth()){
        throw std::invalid_argument("Matrices must have the same dimensions for addition.");
    }
    std::vector<std::vector<double>> result(getHeight(), std::vector<double>(getWidth()));

    for (int i = 0; i < getHeight(); ++i){
        for (int j = 0; j < getWidth(); ++j){
            result[i][j] = (*this)(i, j) + other(i, j);
        }
    }
    return MyMatrix(result);
}

MyMatrix MyMatrix::multiply(const MyMatrix& other) const{
    if (getWidth() != other.getHeight()){
        throw std::invalid_argument("Matrix A columns must equal Matrix B rows for multiplications");
    }

    std::vector<std::vector<double>> result(getHeight(), std::vector<double>(other.getWidth(), 0.0));

    for (int i = 0; i < getHeight(); ++i){
        for (int j = 0; j < other.getWidth(); ++j){
            for (int k = 0; k < getWidth(); ++k){
                result[i][j] += (*this)(i, k) * other(k, j);
            }
        }
    }
    return MyMatrix(result);
}

std::vector<std::vector<double>> MyMatrix::getTransposedArray(){
    std::vector<std::vector<double>> result(getWidth(), std::vector<double>(getHeight()));
    for (int i = 0; i < getHeight(); ++i){
        for (int j = 0; j < getWidth(); ++j){
            result[j][i] = (*this)(i, j);
        }
    }

    return result;
}

MyMatrix MyMatrix::getTransposedCopy(){
    return MyMatrix(getTransposedArray());
}

void MyMatrix::TransposeMe(){
    matrix = getTransposedArray();
    cachedDeterminant = nullptr;
}

double* MyMatrix::CalculateDeterminant() {
    validateSquareSize();

    if (cachedDeterminant != nullptr) {
        return cachedDeterminant;
    }

    std::vector<std::vector<double>> temp = matrix;
    double det = 1.0;

    for (size_t i = 0; i < temp.size(); ++i) {
        size_t maxRow = i;

        for (size_t k = i + 1; k < temp.size(); ++k) {
            if (std::fabs(temp[k][i]) > std::fabs(temp[maxRow][i])) {
                maxRow = k;
            }
        }

        if (std::fabs(temp[maxRow][i]) < 1e-10) {
            cachedDeterminant = new double(0.0);
            return cachedDeterminant;
        }

        if (maxRow != i) {
            std::swap(temp[i], temp[maxRow]);
            det *= -1;
        }

        det *= temp[i][i];

        for (size_t k = i + 1; k < temp.size(); ++k) {
            double factor = temp[k][i] / temp[i][i];
            for (size_t j = i; j < temp[i].size(); ++j) {
                temp[k][j] -= factor * temp[i][j];
            }
        }
    }

    cachedDeterminant = new double(det);
    return cachedDeterminant;
}