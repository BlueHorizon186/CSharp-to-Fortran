! This file is to test the Semantic Analyzer step by step.

      program test

      integer n
      parameter (n=100)
      integer i, biggest, a(n)
      data a/83, 40, 21, 69, 62, 28, 91,  9, 22, 90,
     +       39, 89, 19, 47, 19, 77, 69, 50,  0, 20,
     +       19, 16, 19, 25, 78, 85, 87, 83, 45, 15,
     +       61, 29,  3, 80, 18, 34, 17, 21, 89, 13,
     +       77, 24, 79, 77, 10, 67,  2, 59, 95, 17,
     +       45,  0, 36, 52, 66, 19, 36,  0,  2,  7,
     +       95, 46,  3, 61, 79, 26, 40, 24, 83, 36,
     +       63, 57, 80, 43, 74, 20, 83, 72,  1, 56,
     +       39, 46, 37, 56, 97, 34,  7,  1, 77, 50,
     +       67, 41, 55, 75, 11, 99, 12, 15, 75, 16/

      i = 1

!      integer n, k, m, a(2, 3)
!      parameter (m=100)
!      real i, p

!      n = 5
!      k = 3
!      i = 2.0
!      p = 3.45

      read(*, *) i, biggest
      write(*, *) 'Hello World!'

      stop
      end
