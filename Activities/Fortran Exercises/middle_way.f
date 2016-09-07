      program middle_way
      parameter (arr_length = 3)
      integer a(arr_length), b(arr_length)
      real middle, middle_index

      a(1) = 5
      a(2) = 7
      a(3) = 3

      b(1) = 4
      b(2) = 10
      b(3) = 12

      middle = middle_index(arr_length)

      write(*, *) 'The middle numbers are: '
      write(*, *) '[', a(middle), ', ', b(middle), ']'

      stop
      end

      real function middle_index(len)
        real len

        if (mod(len, 2) .eq. 0) then
            middle_index = len / 2
        else
            middle_index = (len / 2) + 1
        endif

        return
      end
