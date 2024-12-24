#ifndef MATRIXDATA_H
#define MATRIXDATA_H

#include <vector>
#include <string>

class MyMatrix {
private:
    std::vector<std::vector<double>> matrix;
    mutable double* cachedDeterminant = nullptr;

    void validateIndexes(int i, int j) const;
    void validateSquareSize() const;
    void validateJaggedMatrix(std::vector<std::vector<double>> array) const;


public:
    MyMatrix(const MyMatrix& other);
    MyMatrix(const std::vector<std::vector<double>>& array);
    MyMatrix(const std::vector<std::string>& rows);
    MyMatrix(const std::string& matrixString);

    ~MyMatrix();

    int getHeight() const;
    int getWidth() const;
    double getElement(int i, int j) const;
    void setElement(int i, int j, double value);
    double& operator()(int i, int j);
    const double& operator()(int i, int j) const;

    MyMatrix add(const MyMatrix& other) const;
    MyMatrix multiply(const MyMatrix& other) const;
    std::vector<std::vector<double>> getTransposedArray();
    MyMatrix getTransposedCopy();
    void TransposeMe();
    double* CalculateDeterminant();
    std::string toString() const;

};

#endif
