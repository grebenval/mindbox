## ������� �1

### ��������
������ ���������� �� .net 5.
���������� ������� �� ���� �������. ���������� �� ������ ��������� �������� ����� �������� ������������ ��, ��� ������������ ��������� ��,
�������� � ������� ��������� GRPC. 
***������ (�������)***:
1. **MindboxApi** - ���������� Web Api
2. **Mindbox.Bl** - ����� ������, �� ��������� �� �� (������ ������)
3. **Mindbox.Database.Sqlite** - ������ ������� � �� / ������������������ ������ ������� � ��

����������� ��������� **swagger**, ��������� �� ������ /swagger/index.html

� �������� ���� ������ ������� Sqlite. 
ORM - Entity Framework Core - ��� (����) �������� ����������, ������������� � ������������ ***Microsoft***, 
��� ������������ �������� ������������� � ��������. ����� ������������ Code First, ��� ������������ ������.
���� ������ ����� �������� ����� ���������:
1. �������� Entity Framework Core, ������� �� � ���
2. ���� ��������� ������ ORM / ������ ����� ADO.NET - ��������� ������ ���������� ����� ����������� ��������� ***IFigureManipulation***, � 
��� ����������� ����������� ***IDatabaseConnect*** *(���������� ������ �������������)* �� ����.

### �������� �������� ���������
1. ***figureType*** - ��� ������ (1 - *����*, 2 - *�����������*), ��� ������*** int***
2. ***figureInputData*** - ������ ������� ����������, ���� ������*** double***, 1 - �������� ��� �����, 2 - ��������� ��� ������������.

������� �������� JSON, �� ������� ������:
1. ������� ����� 
>{
  "figureType": 1,
  "figureInputData": [
    23.45
  ]
}

2. ������� ����������� 
>{
  "figureType": 1,
  "figureInputData": [
    3,4,5
  ]
}

������� �� ���������� �������� JSON, �� ������� ������:

1. �� ��������� ��� ������
>{
  "figureType": -10,
  "figureInputData": [
    23.45
  ]
}

2. �� ���������� ������ �����
>{
  "figureType": 1,
  "figureInputData": [
    -23.45
  ]
}

3. �� ����� - ������ �����
>{
  "figureType": 1,
  "figureInputData": [
    "NaN"
  ]
}

4. ������������� - ������ �����
>{
  "figureType": 1,
  "figureInputData": [
    "Infinity"
  ]
}

5. �� ����������� 
>{
  "figureType": 1,
  "figureInputData": [
    1,1,5
  ]
}

6. ������������� ����� ������� ����������
>{
  "figureType": 1,
  "figureInputData": [
    1,-7,5
  ]
}

7. ������� ������ �����������, ����� ���� ������� ������ ������ double.MaxValue = 1,7976931348623157E+308. �� �������� ���������� ������� (������� ������� �������� ��� ������� ���������� ��� double)
>{
  "figureType": 2,
  "figureInputData": [
    1.79769313486231E308, 1.7E308, 1.7E308
  ]
}

### �������� ����� ��
����� �� �������� � ����� ***SqliteCreateDatabase.sql*** ���� ���� ������ ***mindbox.sqlite***, ������� **Mindbox.Database.Sqlite**.
������� ������ ������� � ��, ������� ��� ������� ������ (����������, ����� �� ����� � �� ������� ��� ����������� ������ ������ ��� ������� �� ��, � ���� ��� �������). 
����� ����, ����� �������� �� ���������� �������� (�������� ��� ������� ����� ������, ���� ������ ��������� �� ����� ��� �� ����������).
���� ������� ������ ������ � JSON ��� � ������� �� ������ 1 - � �������������, �������� ��-�� ���������� ������������� � �� � �������� 
(���� ��� �������� ��� ����������), � ����� �� �������� ������ ��������� ������� ���� ������ � ��.  
� �� ��� �������:
1. ***figures*** - ������� ��� �������� ����� (�������������, ��� ������, �������)
2. ***circles*** - ������� 1 � 1 � *figures*, �������� ������ � ���� ��� �����
3. ***triangle*** - ������� 1 � 1 � *figures*, �������� ������� ������������ a,b,c � ���� ��� �����

### ***Unit-�����***
������������ XUnit - *�������� ����*:
1. **MindboxApi** - 45%, ������ ���������� (��� ������� ������� ����� 100%, �� ����� �������? ����������������?)
2. **Mindbox.Bl** - 100%.
3. **Mindbox.Database.Sqlite** - 93.88%, ������� ������ ��� ����������� ������ (��, ��� ������������ �� �������, ����� � ��� �������, �� ����� �������? ����������������?)

### ���������� �������� � ���������� (��������� ������ �������� ��������� ������ ���� �����?)

#### ���������� - ���������� ������ �������
1. ��������� **Mindbox.Bl**
* ***���������*** ��������� ����� � ***IFigureManipulation*** ��� ***������� ��������������*** ��� �������������� � ��
* ����������� ������ ��� ������� ������ � ����������
2. ��������� **Mindbox.Bl** - �������� ����� ������� �� � ���������� �� � ������ ������. ��� ������� ����� ������ ������� � ������ ����������� ��������� ��������� �������
��������� ������ (��. ����). ��� ����� ������� ��������� ������������ ������ (database first) ��� code first.
3. ��������� **MindboxApi** - �������� ������ ������������� �����������, ��� �������� ����� ����������.
4. ������� ��� Unit �������, � ������ �������.

#### ���������� - ��������� ������ ���� ������

1. ��������� �� (� ������ **Mindbox.Database.Sqlite**)
* ��� ����������, ���������� �������� � �� ������� �� ������ 1 � 1 � ������� ***figures***, � ����������� ���� ������� ��� �������� ������ ������
��� ����� ������� ��������� ������������ ������ (database first) ��� code first
* � ������ ***FigureExtension***, ����� ***AddSpecFigure***, �������� ��������� ���� ������ ��� ����������, �� �������� ��� ������ �����.

2. ��������� **Mindbox.Bl**
* ������� ����� ��� ������, ����������� �� ���������� ***IFigureInput***, � ����������� ����������� ����� ��� ��������� ***Validate***
(��.  ������ ����� ***FigureTriangle***)
* ����� ***FigureCreate*** - �������� ����� ��� ������, � ***FigureType***, �������� �������� ������ �� ���� � 
����� ***Create()***, ��������� ������ ***_validatorsFigures*** �� ����� ��������� ��������� � ��������� ������ ������, � ����������� ����� ��������� �
������ ***Validate***
3. ��������� **MindboxApi** - �� ���������.
4. ������� ��� Unit �������, � ������ �������.

