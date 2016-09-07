      program matrix_multiply
      parameter (x = 2, y = 3)
      real A(x, y), B(y, x), R(x, x)
      real partial
      data A/1.0, 4.0, 2.0, 5.0, 3.0, 6.0/
      data B/7.0, 9.0, 11.0, 8.0, 10.0, 12.0/

      write(*, *) 'To be written...'

      do 100 i = 1, x
        do 200 j = 1, y
          partial = 0

          do 300 k = 1, y
            partial = partial + (A(i, k) * B(k, j))
300       continue
          
          R(i, j) = partial
200     continue
100   continue

      write(*, *) 'The result is: '

      do 600 i = 1, x
        do 700 j = 1, x
            write(*, *) '[', R(i, j), ']'
700     continue
        write(*, *) ''
600   continue

      stop
      end
