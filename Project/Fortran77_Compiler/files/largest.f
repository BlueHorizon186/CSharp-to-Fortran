! Find the largest of several positive numbers contained in an array.

      program largest

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

!      biggest = -1
!      i = 1
!10    if (a(i) .gt. biggest) then
!          biggest = a(i)
!          write(*, *) 'Largest number found so far:', biggest
!          write(*, *) 'At index', i
!      endif
!      if (i .eq. n) goto 20
!      i = i + 1
!      goto 10
20    stop
      end

